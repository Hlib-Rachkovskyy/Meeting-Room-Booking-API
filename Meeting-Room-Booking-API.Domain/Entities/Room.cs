using Meeting_Room_Booking_API.Domain.Exceptions;

namespace Meeting_Room_Booking_API.Domain.Entities;

public class Room
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public int Capacity { get; private set; }

    private readonly List<Booking> _bookings = new();
    public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

    // EF Core parameterless constructor
    private Room() { }

    public Room(string name, string location, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Room name is required.", nameof(name));

        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");

        Id = Guid.NewGuid();
        Name = name;
        Location = location;
        Capacity = capacity;
    }

    /// <summary>
    /// Attempts to add a booking to this room.
    /// Throws if the requested time slot conflicts with an existing booking.
    /// </summary>
    public void AddBooking(Booking booking)
    {
        if (HasConflict(booking.StartTime, booking.EndTime))
            throw new ConflictException(
                $"Room '{Name}' is already booked for an overlapping time slot " +
                $"({booking.StartTime:g} – {booking.EndTime:g}).");

        _bookings.Add(booking);
    }

    /// <summary>
    /// Returns true when the given time range intersects with any existing booking.
    /// Two intervals [A_start, A_end) and [B_start, B_end) overlap when
    /// A_start &lt; B_end AND B_start &lt; A_end.
    /// </summary>
    public bool HasConflict(DateTime startTime, DateTime endTime)
    {
        return _bookings.Any(existing =>
            startTime < existing.EndTime && existing.StartTime < endTime);
    }
}
