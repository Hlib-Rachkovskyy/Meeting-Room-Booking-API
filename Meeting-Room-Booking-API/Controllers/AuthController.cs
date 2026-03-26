using Meeting_Room_Booking_API.Domain.DTOs;
using Meeting_Room_Booking_API.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Meeting_Room_Booking_API.Controllers;

/// <summary>
/// Handles user registration and login.
/// These endpoints are publicly accessible — no JWT token is required.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="request">The user's full name, email address, and chosen password.</param>
    /// <returns>A JWT token valid for 8 hours, along with basic user info.</returns>
    /// <response code="201">Registration successful. Returns the JWT token.</response>
    /// <response code="409">An account with that email already exists.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var response = await _authService.RegisterAsync(request);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Authenticates an existing user and returns a JWT token.
    /// </summary>
    /// <param name="request">The user's email address and password.</param>
    /// <returns>A JWT token valid for 8 hours, along with basic user info.</returns>
    /// <response code="200">Login successful. Returns the JWT token.</response>
    /// <response code="401">Invalid email or password.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
