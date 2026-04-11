using System.ComponentModel.DataAnnotations;

namespace Meeting_Room_Booking_API.DTOs;

/// <summary>Request body for creating a new meeting room.</summary>
public record CreateRoomRequest(
    [property: Required(AllowEmptyStrings = false, ErrorMessage = "Room name is required.")]
    [property: StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be 2–100 characters.")]
    string Name,

    [property: Required(AllowEmptyStrings = false, ErrorMessage = "Location is required.")]
    [property: StringLength(200, ErrorMessage = "Location must be at most 200 characters.")]
    string Location,

    [property: Range(1, 500, ErrorMessage = "Capacity must be between 1 and 500.")]
    int Capacity
);

/// <summary>
/// Request body for creating a new booking.
/// BookedBy is intentionally absent — it is derived from the authenticated user's JWT claims.
/// </summary>
public record CreateBookingRequest(
    DateTime StartTime,
    DateTime EndTime
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
            yield return new ValidationResult(
                "EndTime must be strictly after StartTime.",
                [nameof(EndTime)]);
    }
}

