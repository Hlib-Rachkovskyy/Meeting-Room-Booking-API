using Meeting_Room_Booking_API.Domain.Entities;
using Meeting_Room_Booking_API.Domain.Interfaces;
using Meeting_Room_Booking_API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Meeting_Room_Booking_API.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationDbContext _context;

    public RoomRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets a room by its ID. Pass includeBookings: true to eager-load the
    /// Bookings collection — required when you need conflict detection.
    /// </summary>
    public async Task<Room?> GetByIdAsync(Guid id, bool includeBookings = false)
    {
        IQueryable<Room> query = _context.Rooms;

        if (includeBookings)
            query = query.Include(r => r.Bookings);

        return await query.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        return await _context.Rooms
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Room room)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var room = await _context.Rooms.FindAsync(id)
            ?? throw new KeyNotFoundException($"Room with ID '{id}' was not found.");

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }
}
