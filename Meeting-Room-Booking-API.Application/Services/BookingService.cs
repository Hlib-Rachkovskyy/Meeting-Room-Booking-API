using Meeting_Room_Booking_API.Domain.Entities;
using Meeting_Room_Booking_API.Domain.Interfaces;

namespace Meeting_Room_Booking_API.Application.Services;

public class BookingService : IBookingService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;

    public BookingService(
        IRoomRepository roomRepository,
        IBookingRepository bookingRepository,
        IUserRepository userRepository)
    {
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
        _userRepository = userRepository;
    }

    public async Task<Booking> BookRoomAsync(Guid roomId, Guid userId, DateTime startTime, DateTime endTime)
    {
        var room = await _roomRepository.GetByIdAsync(roomId, includeBookings: true);
        if (room == null)
            throw new KeyNotFoundException($"Room with ID {roomId} was not found.");

        var user = await _userRepository.GetByIdAsync(userId);
        var bookedBy = user?.FullName ?? userId.ToString();

        var booking = new Booking(roomId, userId, bookedBy, startTime, endTime);

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

    public async Task<bool> CancelBookingAsync(Guid bookingId, Guid requestingUserId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
            return false;

        if (booking.UserId != requestingUserId)
            throw new UnauthorizedAccessException(
                $"You are not authorised to cancel booking '{bookingId}'.");

        await _bookingRepository.DeleteAsync(bookingId);
        return true;
    }
}
