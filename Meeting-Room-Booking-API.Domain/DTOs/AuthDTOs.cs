using System.ComponentModel.DataAnnotations;

namespace Meeting_Room_Booking_API.Domain.DTOs;

/// <summary>Request body for registering a new user.</summary>
public class RegisterRequest
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(100, MinimumLength = 2)]
    public string FullName { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", 
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    public string Password { get; init; } = string.Empty;
}

/// <summary>Request body for logging in.</summary>
public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}


/// <summary>
/// Returned after a successful register or login.
/// <c>AccessToken</c> is a short-lived JWT (15 minutes) to be sent as a Bearer token.
/// A long-lived refresh token (7 days) is set separately as an HttpOnly cookie.
/// </summary>
public record AuthResponse(string AccessToken, string Email, string FullName, DateTime ExpiresAt);

/// <summary>
/// Returned by the /refresh endpoint.
/// Contains a new short-lived access token; the new refresh token is set in an HttpOnly cookie.
/// </summary>
public record RefreshResponse(string AccessToken, DateTime ExpiresAt);
