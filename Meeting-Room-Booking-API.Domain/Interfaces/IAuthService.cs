using Meeting_Room_Booking_API.Domain.DTOs;

namespace Meeting_Room_Booking_API.Domain.Interfaces;

/// <summary>
/// Handles user registration and authentication, returning a signed JWT on success.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user. Throws if the email is already taken.
    /// </summary>
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Validates credentials and returns a signed JWT token.
    /// Throws UnauthorizedAccessException if credentials are invalid.
    /// </summary>
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
