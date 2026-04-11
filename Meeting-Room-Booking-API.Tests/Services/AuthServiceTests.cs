using Meeting_Room_Booking_API.Domain.Entities;
using Meeting_Room_Booking_API.Domain.Interfaces;
using Meeting_Room_Booking_API.Infrastructure.Services;
using Meeting_Room_Booking_API.Domain.DTOs;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Meeting_Room_Booking_API.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly AuthService _authService;

    private const string Secret = "ThisIsADifferentSecretForRefreshTokensOnly456!";
    private const string Issuer = "https://localhost:5001";
    private const string Audience = "https://localhost:5001";

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _configurationMock = new Mock<IConfiguration>();

        // Set up mock configuration for JWT secrets
        _configurationMock.Setup(c => c["JWT_KEY"]).Returns("ThisIsASecretKeyForTestingPurposesOnly123!");
        _configurationMock.Setup(c => c["JWT_REFRESH_KEY"]).Returns(Secret);
        _configurationMock.Setup(c => c["JWT_ISSUER"]).Returns(Issuer);
        _configurationMock.Setup(c => c["JWT_AUDIENCE"]).Returns(Audience);

        _authService = new AuthService(_userRepositoryMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task Logout_IncrementsTokenVersion()
    {
        // Arrange
        var user = new User("test@example.com", "hash", "Test User");
        var initialVersion = user.RefreshTokenVersion;

        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        // Act
        await _authService.LogoutAsync(user.Id);

        // Assert
        Assert.Equal(initialVersion + 1, user.RefreshTokenVersion);
        _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task Refresh_Throws_WhenVersionMismatch()
    {
        // Arrange
        var user = new User("test@example.com", "hash", "Test User");
        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        // Generate a refresh token with current version (0)
        var tokenWithVersion0 = GenerateManualRefreshToken(user.Id, 0);

        // Bump version in user object (simulating what LogoutAsync does)
        user.IncrementRefreshTokenVersion(); // Now version is 1

        // Act & Assert
        var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _authService.RefreshAsync(tokenWithVersion0));
        
        Assert.Contains("invalid", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Refresh_Succeeds_WithValidToken()
    {
        // Arrange
        var user = new User("test@example.com", "hash", "Test User");
        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        // Create a token through the regular flow
        // BuildTokenPair is private, so we'll just mock the login behavior
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);
        
        // Use a real token generated with version 0
        var validToken = GenerateManualRefreshToken(user.Id, 0);

        // Act
        var (refreshResponse, _) = await _authService.RefreshAsync(validToken);

        // Assert
        Assert.NotNull(refreshResponse.AccessToken);
    }

    private string GenerateManualRefreshToken(Guid userId, int version)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("rtv", version.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

