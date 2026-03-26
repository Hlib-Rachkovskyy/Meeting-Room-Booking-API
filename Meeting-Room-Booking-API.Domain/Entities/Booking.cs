namespace Meeting_Room_Booking_API.Domain.Entities;

public class Booking
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    // EF Core parameterless constructor
    private Booking() { }

    public Booking(Guid roomId, string bookedBy, DateTime startTime, DateTime endTime)
    {
        if (roomId == Guid.Empty)
            throw new ArgumentException("A valid room ID is required.", nameof(roomId));

        if (string.IsNullOrWhiteSpace(bookedBy))
            throw new ArgumentException("BookedBy is required.", nameof(bookedBy));

        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time.");

        Id = Guid.NewGuid();
        RoomId = roomId;
        BookedBy = bookedBy;
        StartTime = startTime;
        EndTime = endTime;
    }
}
