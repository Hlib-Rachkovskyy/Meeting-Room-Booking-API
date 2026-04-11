using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Meeting_Room_Booking_API.Controllers;
using Meeting_Room_Booking_API.Domain.Entities;
using Meeting_Room_Booking_API.Domain.Interfaces;
using Meeting_Room_Booking_API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Meeting_Room_Booking_API.Tests.Controllers;

public class BookingsControllerTests
{
    private readonly Mock<IBookingService> _bookingServiceMock;
    private readonly BookingsController _controller;

    private static readonly Guid AuthUserId = Guid.NewGuid();
    private static readonly Guid RoomId = Guid.NewGuid();

    public BookingsControllerTests()
    {
        _bookingServiceMock = new Mock<IBookingService>();
        _controller = new BookingsController(_bookingServiceMock.Object);

        // Simulate an authenticated user with the AuthUserId as the 'sub' claim
        var claims = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(JwtRegisteredClaimNames.Sub, AuthUserId.ToString()),
        ], "Test"));
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claims }
        };
    }

    // ── Create ───────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Create_UsesAuthenticatedUserIdAsBookedBy_NotRequestBody()
    {
        // Arrange
        var start = DateTime.UtcNow.AddHours(1);
        var request = new CreateBookingRequest(start, start.AddHours(1));

        var expectedBooking = new Booking(RoomId, AuthUserId, "Test User", start, start.AddHours(1));
        _bookingServiceMock
            .Setup(s => s.BookRoomAsync(RoomId, AuthUserId, start, start.AddHours(1)))
            .ReturnsAsync(expectedBooking);

        // Act
        var result = await _controller.Create(RoomId, request);

        // Assert — service was called with the JWT user ID, not a DTO field
        _bookingServiceMock.Verify(
            s => s.BookRoomAsync(RoomId, AuthUserId, start, start.AddHours(1)),
            Times.Once);
        Assert.IsType<CreatedAtActionResult>(result);
    }

    // ── Cancel ───────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Cancel_Returns204_WhenUserOwnsBooking()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        _bookingServiceMock
            .Setup(s => s.CancelBookingAsync(bookingId, AuthUserId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Cancel(RoomId, bookingId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Cancel_Returns403_WhenUserDoesNotOwnBooking()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        _bookingServiceMock
            .Setup(s => s.CancelBookingAsync(bookingId, AuthUserId))
            .ThrowsAsync(new UnauthorizedAccessException("You do not own this booking."));

        // Act
        var result = await _controller.Cancel(RoomId, bookingId);

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status403Forbidden, statusResult.StatusCode);
    }

    [Fact]
    public async Task Cancel_Returns404_WhenBookingDoesNotExist()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        _bookingServiceMock
            .Setup(s => s.CancelBookingAsync(bookingId, AuthUserId))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Cancel(RoomId, bookingId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
