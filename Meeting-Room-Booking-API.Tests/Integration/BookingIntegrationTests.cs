using Meeting_Room_Booking_API.Application.Services;
using Meeting_Room_Booking_API.Domain.Entities;
using Meeting_Room_Booking_API.Infrastructure.Data;
using Meeting_Room_Booking_API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Meeting_Room_Booking_API.Tests.Integration;

public class BookingIntegrationTests
{
    [Fact]
    public async Task BookRoomAsync_ShouldSuccessfullyCreateBooking()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var roomRepository = new RoomRepository(context);
        var userRepository = new UserRepository(context);
        var bookingRepository = new BookingRepository(context);
        var bookingService = new BookingService(roomRepository, bookingRepository, userRepository);

        var room = new Room("Apollo", "Floor 1", 10);
        context.Rooms.Add(room);
        
        var user = new User("test@example.com", "hash", "Test User");
        context.Users.Add(user);
        
        await context.SaveChangesAsync();

        // Act
        // This is where it's expected to throw if our hypothesis is correct
        var booking = await bookingService.BookRoomAsync(
            room.Id, 
            user.Id, 
            DateTime.UtcNow.AddHours(1), 
            DateTime.UtcNow.AddHours(2));

        // Assert
        Assert.NotNull(booking);
        var updatedRoom = await context.Rooms.Include(r => r.Bookings).FirstAsync(r => r.Id == room.Id);
        Assert.Single(updatedRoom.Bookings);
    }
}
