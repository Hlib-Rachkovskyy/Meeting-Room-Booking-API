using Meeting_Room_Booking_API.Domain.Entities;
using Meeting_Room_Booking_API.Domain.Interfaces;

namespace Meeting_Room_Booking_API.Application.Services;

public class BookingService : IBookingService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IRoomRepository roomRepository, IBookingRepository bookingRepository)
    {
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<Booking> BookRoomAsync(Guid roomId, string bookedBy, DateTime startTime, DateTime endTime)
    {
        var room = await _roomRepository.GetByIdAsync(roomId, includeBookings: true);
        if (room == null)
            throw new KeyNotFoundException($"Room with ID {roomId} was not found.");

        var booking = new Booking(roomId, bookedBy, startTime, endTime);
        
        // This will throw if there's a conflict
        room.AddBooking(booking);

        await _roomRepository.UpdateAsync(room);
        
        return booking;
    }

    public async Task<IEnumerable<Booking>> GetBookingsByRoomAsync(Guid roomId)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null)
            throw new KeyNotFoundException($"Room with ID {roomId} was not found.");

        return await _bookingRepository.GetByRoomIdAsync(roomId);
    }

    public async Task<bool> CancelBookingAsync(Guid bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
            return false;

        await _bookingRepository.DeleteAsync(bookingId);
        return true;
    }
}
