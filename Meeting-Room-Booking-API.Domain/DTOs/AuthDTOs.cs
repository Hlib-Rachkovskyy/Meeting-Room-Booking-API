namespace Meeting_Room_Booking_API.Domain.DTOs;

/// <summary>Request body for registering a new user.</summary>
public record RegisterRequest(string FullName, string Email, string Password);

/// <summary>Request body for logging in.</summary>
public record LoginRequest(string Email, string Password);

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
