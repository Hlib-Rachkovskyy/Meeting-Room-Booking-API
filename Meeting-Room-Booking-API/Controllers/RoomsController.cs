using Meeting_Room_Booking_API.Domain.Entities;
using Meeting_Room_Booking_API.Domain.Interfaces;
using Meeting_Room_Booking_API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meeting_Room_Booking_API.Controllers;

/// <summary>
/// Manages meeting room resources.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class RoomsController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;

    public RoomsController(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// Retrieves all meeting rooms.
    /// </summary>
    /// <returns>A list of all available meeting rooms.</returns>
    /// <response code="200">Returns the list of rooms.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Room>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await _roomRepository.GetAllAsync();
        return Ok(rooms);
    }

    /// <summary>
    /// Retrieves a specific meeting room by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the room.</param>
    /// <returns>The room that matches the given ID.</returns>
    /// <response code="200">Returns the matching room.</response>
    /// <response code="404">Room was not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Room), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var room = await _roomRepository.GetByIdAsync(id, includeBookings: true);
        if (room is null)
            return NotFound(new { message = $"Room with ID '{id}' was not found." });

        return Ok(room);
    }

    /// <summary>
    /// Creates a new meeting room.
    /// </summary>
    /// <param name="request">The details of the room to create.</param>
    /// <returns>The newly created room.</returns>
    /// <response code="201">Room was created successfully.</response>
    /// <response code="400">Validation error in the request body.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Room), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
    {
        var room = new Room(request.Name, request.Location, request.Capacity);
        await _roomRepository.AddAsync(room);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    /// <summary>
    /// Deletes a meeting room by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the room to delete.</param>
    /// <response code="204">Room deleted successfully.</response>
    /// <response code="404">Room was not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room is null)
            return NotFound(new { message = $"Room with ID '{id}' was not found." });

        await _roomRepository.DeleteAsync(id);
        return NoContent();
    }
}
