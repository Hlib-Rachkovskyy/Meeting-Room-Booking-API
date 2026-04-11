namespace Meeting_Room_Booking_API.Domain.Entities;

public class Booking
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    /// <summary>The ID of the authenticated user who created this booking.</summary>
    public Guid UserId { get; private set; }
    /// <summary>Free-text display name (derived from the user's full name at booking time).</summary>
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    // EF Core parameterless constructor
    private Booking() { }

    public Booking(Guid roomId, Guid userId, string bookedBy, DateTime startTime, DateTime endTime)
    {
        if (roomId == Guid.Empty)
            throw new ArgumentException("A valid room ID is required.", nameof(roomId));

        if (userId == Guid.Empty)
            throw new ArgumentException("A valid user ID is required.", nameof(userId));

        if (string.IsNullOrWhiteSpace(bookedBy))
            throw new ArgumentException("BookedBy is required.", nameof(bookedBy));

        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time.");

        Id = Guid.NewGuid();
        RoomId = roomId;
        UserId = userId;
        BookedBy = bookedBy;
        StartTime = startTime;
        EndTime = endTime;
    }
}
