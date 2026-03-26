namespace Meeting_Room_Booking_API.Domain.DTOs;

/// <summary>Request body for registering a new user.</summary>
public record RegisterRequest(string FullName, string Email, string Password);

/// <summary>Request body for logging in.</summary>
public record LoginRequest(string Email, string Password);

/// <summary>Returned after successful register or login.</summary>
public record AuthResponse(string Token, string Email, string FullName, DateTime ExpiresAt);
