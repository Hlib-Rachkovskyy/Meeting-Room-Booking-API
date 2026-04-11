using Meeting_Room_Booking_API.Domain.DTOs;
using Meeting_Room_Booking_API.Domain.Entities;
using Meeting_Room_Booking_API.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Meeting_Room_Booking_API.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;

        // Startup Guard: Validate required keys exist
        GetConfigValue("JWT_KEY");
        GetConfigValue("JWT_REFRESH_KEY");
        GetConfigValue("JWT_ISSUER");
        GetConfigValue("JWT_AUDIENCE");
    }

    // ── Public interface methods ────────────────────────────────────────────────

    public async Task<(AuthResponse Auth, string RefreshToken)> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email))
            throw new InvalidOperationException($"An account with email '{request.Email}' already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12);
        var user = new User(request.Email, passwordHash, request.FullName);

        await _userRepository.AddAsync(user);

        return BuildTokenPair(user);
    }

    public async Task<(AuthResponse Auth, string RefreshToken)> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (user.IsLockedOut())
            throw new UnauthorizedAccessException("Account locked out due to too many failed attempts. Try again later.");

        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!passwordValid)
        {
            user.RecordFailedLogin();
            await _userRepository.UpdateAsync(user);
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        user.ResetFailedLogins();
        await _userRepository.UpdateAsync(user);

        return BuildTokenPair(user);
    }

    public async Task<(RefreshResponse Refresh, string NewRefreshToken)> RefreshAsync(string refreshToken)
    {
        var userId = ValidateRefreshToken(refreshToken);

        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new UnauthorizedAccessException("User associated with this token no longer exists.");

        // Invalidate old refresh token to prevent replay attacks
        user.IncrementRefreshTokenVersion();
        await _userRepository.UpdateAsync(user);

        var accessToken = GenerateAccessToken(user, out var expiresAt);
        var newRefreshToken = GenerateRefreshToken(user, user.Id);

        return (new RefreshResponse(accessToken, expiresAt), newRefreshToken);
    }

    public async Task LogoutAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new UnauthorizedAccessException("User not found.");

        user.IncrementRefreshTokenVersion();
        await _userRepository.UpdateAsync(user);
    }

    // ── Private helpers ─────────────────────────────────────────────────────────

    private (AuthResponse Auth, string RefreshToken) BuildTokenPair(User user)
    {
        var accessToken = GenerateAccessToken(user, out var expiresAt);
        var refreshToken = GenerateRefreshToken(user, user.Id);
        var authResponse = new AuthResponse(accessToken, user.Email, user.FullName, expiresAt);
        return (authResponse, refreshToken);
    }

    private string GenerateAccessToken(User user, out DateTime expiresAt)
    {
        var key = GetConfigValue("JWT_KEY");
        var issuer = GetConfigValue("JWT_ISSUER");
        var audience = GetConfigValue("JWT_AUDIENCE");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        expiresAt = DateTime.UtcNow.AddMinutes(15);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role), // RBAC
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken(User user, Guid userId)
    {
        var key = GetConfigValue("JWT_REFRESH_KEY");
        var issuer = GetConfigValue("JWT_ISSUER");
        var audience = GetConfigValue("JWT_AUDIENCE");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("rtv", user.RefreshTokenVersion.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private Guid ValidateRefreshToken(string refreshToken)
    {
        var key = GetConfigValue("JWT_REFRESH_KEY");
        var issuer = GetConfigValue("JWT_ISSUER");
        var audience = GetConfigValue("JWT_AUDIENCE");

        var validationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(refreshToken, validationParams, out _);

            var sub = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? throw new UnauthorizedAccessException("Refresh token missing subject claim.");

            var userId = Guid.Parse(sub);
            var userTokenVersion = principal.FindFirst("rtv")?.Value;

            // Load user to check current version
            var user = _userRepository.GetByIdAsync(userId).GetAwaiter().GetResult()
                ?? throw new UnauthorizedAccessException("User associated with this token no longer exists.");

            if (userTokenVersion == null || int.Parse(userTokenVersion) != user.RefreshTokenVersion)
            {
                throw new UnauthorizedAccessException("Refresh token is invalid or has been revoked.");
            }

            return userId;
        }
        catch (SecurityTokenExpiredException)
        {
            throw new UnauthorizedAccessException("Refresh token has expired. Please log in again.");
        }
        catch (SecurityTokenException)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }
    }

    private string GetConfigValue(string key)
        => _configuration[key] ?? throw new InvalidOperationException($"Configuration key '{key}' is missing.");
}
