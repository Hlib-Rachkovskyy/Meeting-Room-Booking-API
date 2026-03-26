using Meeting_Room_Booking_API.Domain.Entities;
using Meeting_Room_Booking_API.Domain.Interfaces;
using Meeting_Room_Booking_API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Meeting_Room_Booking_API.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await _context.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _context.Bookings
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Returns all bookings associated with a specific room, ordered chronologically.
    /// </summary>
    public async Task<IEnumerable<Booking>> GetByRoomIdAsync(Guid roomId)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Where(b => b.RoomId == roomId)
            .OrderBy(b => b.StartTime)
            .ToListAsync();
    }

    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var booking = await _context.Bookings.FindAsync(id)
            ?? throw new KeyNotFoundException($"Booking with ID '{id}' was not found.");

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
    }
}
