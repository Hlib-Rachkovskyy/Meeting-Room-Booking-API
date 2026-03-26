using Bogus;
using Meeting_Room_Booking_API.Domain.Entities;
using Meeting_Room_Booking_API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Meeting_Room_Booking_API.Infrastructure.Seeding;

/// <summary>
/// Populates the database with realistic dummy data on first application startup.
/// Uses a fixed seed for reproducible results across runs (while using InMemory DB).
/// </summary>
public static class DatabaseSeeder
{
    private const int RoomCount = 10;
    private const int BookingCount = 50;
    private const int RandomSeed = 12345;

    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        // Only seed if the database is empty
        if (await context.Rooms.AnyAsync())
        {
            logger.LogInformation("Database already seeded. Skipping.");
            return;
        }

        logger.LogInformation("Seeding database with {RoomCount} rooms and {BookingCount} bookings...",
            RoomCount, BookingCount);

        var rooms = GenerateRooms();
        var bookings = GenerateBookings(rooms);

        context.Rooms.AddRange(rooms);
        context.Bookings.AddRange(bookings);
        await context.SaveChangesAsync();

        logger.LogInformation("Database seeding completed successfully.");
    }

    private static List<Room> GenerateRooms()
    {
        var roomNames = new[]
        {
            "Apollo", "Gemini", "Orion", "Titan", "Nova",
            "Horizon", "Summit", "Nebula", "Pulsar", "Eclipse"
        };

        var locations = new[]
        {
            "Floor 1 - East Wing", "Floor 1 - West Wing",
            "Floor 2 - East Wing", "Floor 2 - West Wing",
            "Floor 3 - North", "Floor 3 - South",
            "Floor 4 - East Wing", "Floor 4 - West Wing",
            "Floor 5 - Executive Suite", "Floor 5 - Innovation Lab"
        };

        var capacityFaker = new Faker { Random = new Randomizer(RandomSeed) };

        var rooms = new List<Room>();
        for (var i = 0; i < RoomCount; i++)
        {
            var capacity = capacityFaker.Random.Int(4, 20);
            rooms.Add(new Room(roomNames[i], locations[i], capacity));
        }

        return rooms;
    }

    private static List<Booking> GenerateBookings(List<Room> rooms)
    {
        var faker = new Faker { Random = new Randomizer(RandomSeed + 1) };

        var organizers = new[]
        {
            "Alice Johnson", "Bob Smith", "Charlie Brown", "Diana Prince",
            "Ethan Hunt", "Fiona Green", "George Martin", "Hannah Lee",
            "Isaac Newton", "Julia Roberts", "Kevin Hart", "Laura Palmer"
        };

        var bookings = new List<Booking>();
        var bookingsPerRoom = new Dictionary<Guid, List<(DateTime Start, DateTime End)>>();

        // Initialize tracking dictionary
        foreach (var room in rooms)
        {
            bookingsPerRoom[room.Id] = new List<(DateTime, DateTime)>();
        }

        var attempts = 0;
        const int maxAttempts = 500; // safety net to avoid infinite loops

        while (bookings.Count < BookingCount && attempts < maxAttempts)
        {
            attempts++;

            // Pick a random room
            var room = faker.PickRandom(rooms);

            // Generate a random date within the last 30 days to 14 days in the future
            var date = faker.Date.BetweenDateOnly(
                DateOnly.FromDateTime(DateTime.Today.AddDays(-30)),
                DateOnly.FromDateTime(DateTime.Today.AddDays(14)));

            // Generate a realistic office-hours start time (08:00 – 16:00)
            var startHour = faker.Random.Int(8, 16);
            var startMinute = faker.PickRandom(0, 15, 30, 45);
            var startTime = date.ToDateTime(new TimeOnly(startHour, startMinute));

            // Duration: 30min, 1h, 1.5h, or 2h
            var durationMinutes = faker.PickRandom(30, 60, 90, 120);
            var endTime = startTime.AddMinutes(durationMinutes);

            // Cap at 18:00 (end of business hours)
            var endOfDay = date.ToDateTime(new TimeOnly(18, 0));
            if (endTime > endOfDay)
                continue;

            // Check for conflicts with already-generated bookings for this room
            var existingSlots = bookingsPerRoom[room.Id];
            var hasConflict = existingSlots.Any(slot =>
                startTime < slot.End && slot.Start < endTime);

            if (hasConflict)
                continue;

            // No conflict — create the booking
            var organizer = faker.PickRandom(organizers);
            var booking = new Booking(room.Id, organizer, startTime, endTime);

            bookings.Add(booking);
            existingSlots.Add((startTime, endTime));
        }

        return bookings;
    }
}
