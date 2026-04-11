using Meeting_Room_Booking_API.Domain.DTOs;

namespace Meeting_Room_Booking_API.Domain.Interfaces;

/// <summary>
/// Handles user registration, authentication, and token refresh.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user. Throws <see cref="InvalidOperationException"/> if the email is already taken.
    /// Returns an <see cref="AuthResponse"/> (access token) and a separate refresh token string.
    /// </summary>
    Task<(AuthResponse Auth, string RefreshToken)> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Validates credentials and returns an access token plus a refresh token string.
    /// Throws <see cref="UnauthorizedAccessException"/> if credentials are invalid.
    /// </summary>
    Task<(AuthResponse Auth, string RefreshToken)> LoginAsync(LoginRequest request);

    /// <summary>
    /// Validates the given refresh token JWT, loads the user, and issues a new access token
    /// plus a rotated refresh token. Throws <see cref="UnauthorizedAccessException"/> if
    /// the token is invalid or the user no longer exists.
    /// </summary>
    Task<(RefreshResponse Refresh, string NewRefreshToken)> RefreshAsync(string refreshToken);

    /// <summary>
    /// Revokes all active tokens for the user by incrementing their RefreshTokenVersion.
    /// </summary>
    Task LogoutAsync(Guid userId);
}

