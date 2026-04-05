using Meeting_Room_Booking_API.Domain.DTOs;
using Meeting_Room_Booking_API.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meeting_Room_Booking_API.Controllers;

/// <summary>
/// Handles user registration, login, token refresh, and logout.
/// These endpoints are publicly accessible — no JWT access token is required.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private const string RefreshTokenCookieName = "refreshToken";

    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="request">The user's full name, email address, and chosen password.</param>
    /// <returns>
    /// A short-lived access token (15 minutes) in the response body, and a long-lived
    /// refresh token (7 days) set as an HttpOnly cookie.
    /// </returns>
    /// <response code="201">Registration successful. Returns the access token.</response>
    /// <response code="409">An account with that email already exists.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var (auth, refreshToken) = await _authService.RegisterAsync(request);
            SetRefreshTokenCookie(refreshToken);
            return StatusCode(StatusCodes.Status201Created, auth);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Authenticates an existing user and returns a JWT access token.
    /// </summary>
    /// <param name="request">The user's email address and password.</param>
    /// <returns>
    /// A short-lived access token (15 minutes) in the response body, and a long-lived
    /// refresh token (7 days) set as an HttpOnly cookie.
    /// </returns>
    /// <response code="200">Login successful. Returns the access token.</response>
    /// <response code="401">Invalid email or password.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var (auth, refreshToken) = await _authService.LoginAsync(request);
            SetRefreshTokenCookie(refreshToken);
            return Ok(auth);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Issues a new access token using the HttpOnly refresh token cookie.
    /// Implements refresh token rotation — a new refresh token cookie is also set.
    /// </summary>
    /// <returns>A new short-lived access token (15 minutes).</returns>
    /// <response code="200">Token refreshed successfully. Returns the new access token.</response>
    /// <response code="401">The refresh token cookie is missing, invalid, or expired.</response>
    /// <remarks>
    /// This endpoint reads the refresh token from the <c>refreshToken</c> HttpOnly cookie
    /// automatically sent by the browser. It cannot be tested directly from Swagger UI.
    /// Use Postman (with cookie jar enabled) or an integration test.
    /// </remarks>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(RefreshResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies[RefreshTokenCookieName];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(new { message = "Refresh token cookie is missing." });

        try
        {
            var (refresh, newRefreshToken) = await _authService.RefreshAsync(refreshToken);
            SetRefreshTokenCookie(newRefreshToken);
            return Ok(refresh);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Logs the user out by clearing the refresh token cookie.
    /// </summary>
    /// <response code="204">Logged out successfully. The refresh token cookie has been cleared.</response>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
        return NoContent();
    }

    // ── Private helpers ──────────────────────────────────────────────────────────

    private void SetRefreshTokenCookie(string refreshToken)
    {
        Response.Cookies.Append(RefreshTokenCookieName, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
    }
}
