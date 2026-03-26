using Meeting_Room_Booking_API.Domain.Entities;

namespace Meeting_Room_Booking_API.Domain.Interfaces;

/// <summary>
/// Defines the core booking operations.
/// Implementations must enforce the business rule:
///   A room cannot be double-booked for intersecting time slots.
/// </summary>
public interface IBookingService
{
    /// <summary>
    /// Books the specified room for the given time range.
    /// Must reject the request if the slot overlaps with an existing booking.
    /// </summary>
    Task<Booking> BookRoomAsync(Guid roomId, string bookedBy, DateTime startTime, DateTime endTime);

    /// <summary>
    /// Returns all bookings for a specific room.
    /// </summary>
    Task<IEnumerable<Booking>> GetBookingsByRoomAsync(Guid roomId);

    /// <summary>
    /// Cancels an existing booking by its ID.
    /// </summary>
    Task<bool> CancelBookingAsync(Guid bookingId);
}
