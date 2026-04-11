using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Meeting_Room_Booking_API.Domain.Entities;
using Meeting_Room_Booking_API.Domain.Interfaces;
using Meeting_Room_Booking_API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meeting_Room_Booking_API.Controllers;

/// <summary>
/// Manages room bookings. Enforces the business rule that a room
/// cannot be double-booked for intersecting time slots.
/// </summary>
[ApiController]
[Route("api/rooms/{roomId:guid}/bookings")]
[Authorize]
[Produces("application/json")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    /// <summary>
    /// Retrieves all bookings for a specific meeting room.
    /// </summary>
    /// <param name="roomId">The unique identifier of the room.</param>
    /// <returns>A list of bookings for the given room.</returns>
    /// <response code="200">Returns the list of bookings for the room.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Booking>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRoom(Guid roomId)
    {
        var bookings = await _bookingService.GetBookingsByRoomAsync(roomId);
        return Ok(bookings);
    }

    /// <summary>
    /// Creates a new booking for a specific meeting room.
    /// Returns 400 if the requested time slot overlaps with an existing booking.
    /// </summary>
    /// <param name="roomId">The unique identifier of the room to book.</param>
    /// <param name="request">The booking details (organizer, start and end time).</param>
    /// <returns>The newly created booking.</returns>
    /// <response code="201">Booking was created successfully.</response>
    /// <response code="400">The time slot overlaps with an existing booking, or the request is invalid.</response>
    /// <response code="404">The room was not found.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(Guid roomId, [FromBody] CreateBookingRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID claim missing from token."));

        var booking = await _bookingService.BookRoomAsync(
            roomId,
            userId,
            request.StartTime,
            request.EndTime);

        return CreatedAtAction(
            nameof(GetByRoom),
            new { roomId },
            booking);
    }

    /// <summary>
    /// Cancels an existing booking by its ID.
    /// </summary>
    /// <param name="roomId">The unique identifier of the room (used for route scoping).</param>
    /// <param name="bookingId">The unique identifier of the booking to cancel.</param>
    /// <response code="204">Booking cancelled successfully.</response>
    /// <response code="403">The authenticated user does not own this booking.</response>
    /// <response code="404">Booking was not found.</response>
    [HttpDelete("{bookingId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(Guid roomId, Guid bookingId)
    {
        var userId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID claim missing from token."));

        try
        {
            var cancelled = await _bookingService.CancelBookingAsync(bookingId, userId);
            if (!cancelled)
                return NotFound(new { message = $"Booking with ID '{bookingId}' was not found." });

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
    }
}
