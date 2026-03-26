namespace Meeting_Room_Booking_API.DTOs;

/// <summary>Request body for creating a new meeting room.</summary>
public record CreateRoomRequest(
    string Name,
    string Location,
    int Capacity
);

/// <summary>Request body for creating a new booking.</summary>
public record CreateBookingRequest(
    string BookedBy,
    DateTime StartTime,
    DateTime EndTime
);
