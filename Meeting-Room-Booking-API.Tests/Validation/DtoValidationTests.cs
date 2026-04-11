using System.ComponentModel.DataAnnotations;
using Meeting_Room_Booking_API.Domain.DTOs;
using Meeting_Room_Booking_API.DTOs;

namespace Meeting_Room_Booking_API.Tests.Validation;

/// <summary>
/// Validates DTOs using the same DataAnnotations validator that ASP.NET Core's
/// model binding pipeline uses, so these tests faithfully reflect API behaviour.
/// </summary>
public class DtoValidationTests
{
    private static IList<ValidationResult> Validate(object dto)
    {
        var context = new ValidationContext(dto);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, context, results, validateAllProperties: true);
        return results;
    }

    // ── CreateRoomRequest ────────────────────────────────────────────────────────

    [Fact]
    public void CreateRoomRequest_IsValid_WithGoodData()
    {
        var dto = new CreateRoomRequest("Boardroom A", "Floor 3", 10);
        Assert.Empty(Validate(dto));
    }

    [Fact]
    public void CreateRoomRequest_FailsValidation_WhenNameIsEmpty()
    {
        var dto = new CreateRoomRequest("", "Floor 3", 10);
        Assert.NotEmpty(Validate(dto));
    }

    [Fact]
    public void CreateRoomRequest_FailsValidation_WhenCapacityIsZero()
    {
        var dto = new CreateRoomRequest("Room A", "Floor 1", 0);
        Assert.NotEmpty(Validate(dto));
    }

    [Fact]
    public void CreateRoomRequest_FailsValidation_WhenCapacityIsNegative()
    {
        var dto = new CreateRoomRequest("Room A", "Floor 1", -5);
        Assert.NotEmpty(Validate(dto));
    }

    // ── CreateBookingRequest ─────────────────────────────────────────────────────

    [Fact]
    public void CreateBookingRequest_IsValid_WithFutureTimes()
    {
        var start = DateTime.UtcNow.AddHours(1);
        var dto = new CreateBookingRequest(start, start.AddHours(1));
        Assert.Empty(Validate(dto));
    }

    [Fact]
    public void CreateBookingRequest_FailsValidation_WhenEndTimeBeforeStartTime()
    {
        var start = DateTime.UtcNow.AddHours(2);
        var dto = new CreateBookingRequest(start, start.AddHours(-1)); // end < start
        Assert.NotEmpty(Validate(dto));
    }

    [Fact]
    public void CreateBookingRequest_FailsValidation_WhenEndTimeEqualsStartTime()
    {
        var time = DateTime.UtcNow.AddHours(1);
        var dto = new CreateBookingRequest(time, time);
        Assert.NotEmpty(Validate(dto));
    }

    // ── RegisterRequest ──────────────────────────────────────────────────────────

    [Fact]
    public void RegisterRequest_IsValid_WithGoodData()
    {
        var dto = new RegisterRequest { FullName = "Jane Doe", Email = "jane@example.com", Password = "SecureP@ssword1!" };
        Assert.Empty(Validate(dto));
    }

    [Fact]
    public void RegisterRequest_FailsValidation_WhenPasswordTooShort()
    {
        var dto = new RegisterRequest { FullName = "Jane", Email = "jane@example.com", Password = "short" };
        Assert.NotEmpty(Validate(dto));
    }

    [Fact]
    public void RegisterRequest_FailsValidation_WhenEmailIsInvalid()
    {
        var dto = new RegisterRequest { FullName = "Jane", Email = "not-an-email", Password = "SecureP@ssword1!" };
        Assert.NotEmpty(Validate(dto));
    }

    [Fact]
    public void RegisterRequest_FailsValidation_WhenFullNameIsEmpty()
    {
        var dto = new RegisterRequest { FullName = "", Email = "jane@example.com", Password = "SecureP@ssword1!" };
        Assert.NotEmpty(Validate(dto));
    }

    // ── LoginRequest ─────────────────────────────────────────────────────────────

    [Fact]
    public void LoginRequest_FailsValidation_WhenEmailIsInvalid()
    {
        var dto = new LoginRequest { Email = "not-an-email", Password = "password" };
        Assert.NotEmpty(Validate(dto));
    }

    [Fact]
    public void LoginRequest_FailsValidation_WhenPasswordIsEmpty()
    {
        var dto = new LoginRequest { Email = "user@example.com", Password = "" };
        Assert.NotEmpty(Validate(dto));
    }
}
