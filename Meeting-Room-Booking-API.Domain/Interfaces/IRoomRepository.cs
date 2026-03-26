using Meeting_Room_Booking_API.Domain.Entities;

namespace Meeting_Room_Booking_API.Domain.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(Guid id, bool includeBookings = false);
    Task<IEnumerable<Room>> GetAllAsync();
    Task AddAsync(Room room);
    Task UpdateAsync(Room room);
    Task DeleteAsync(Guid id);
}
