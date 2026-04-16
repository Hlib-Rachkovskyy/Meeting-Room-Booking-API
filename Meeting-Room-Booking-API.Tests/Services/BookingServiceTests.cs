using Meeting_Room_Booking_API.Application.Services;
using Meeting_Room_Booking_API.Domain.Entities;
using Meeting_Room_Booking_API.Domain.Exceptions;
using Meeting_Room_Booking_API.Domain.Interfaces;
using Moq;
using Xunit;

namespace Meeting_Room_Booking_API.Tests.Services;

public class BookingServiceTests
{
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _bookingService = new BookingService(
            _roomRepositoryMock.Object,
            _bookingRepositoryMock.Object,
            _userRepositoryMock.Object);
    }

    [Fact]
    public async Task BookRoomAsync_Succeeds_WhenNoConflicts()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var room = new Room("Test Room", "Location", 10);
        var user = new User("test@example.com", "hash", "Test User");

        _roomRepositoryMock.Setup(r => r.GetByIdAsync(roomId, true)).ReturnsAsync(room);
        _userRepositoryMock.Setup(u => u.GetByIdAsync(userId)).ReturnsAsync(user);

        var startTime = DateTime.UtcNow.AddHours(1);
        var endTime = DateTime.UtcNow.AddHours(2);

        // Act
        var result = await _bookingService.BookRoomAsync(roomId, userId, startTime, endTime);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roomId, result.RoomId);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(user.FullName, result.BookedBy);
        _bookingRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Booking>()), Times.Once);
    }

    [Fact]
    public async Task BookRoomAsync_ThrowsKeyNotFoundException_WhenRoomDoesNotExist()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        _roomRepositoryMock.Setup(r => r.GetByIdAsync(roomId, true)).ReturnsAsync((Room?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _bookingService.BookRoomAsync(roomId, Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddHours(1)));
    }

    [Fact]
    public async Task BookRoomAsync_ThrowsConflictException_WhenRoomAlreadyBooked()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var room = new Room("Test Room", "Location", 10);
        
        // Add an existing booking to trigger conflict
        var existingStart = DateTime.UtcNow.AddHours(1);
        var existingEnd = DateTime.UtcNow.AddHours(2);
        room.AddBooking(new Booking(roomId, Guid.NewGuid(), "Someone", existingStart, existingEnd));

        _roomRepositoryMock.Setup(r => r.GetByIdAsync(roomId, true)).ReturnsAsync(room);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() =>
            _bookingService.BookRoomAsync(roomId, userId, existingStart, existingEnd));
    }

    [Fact]
    public async Task CancelBookingAsync_ReturnsTrue_WhenOwnerCancels()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var booking = new Booking(Guid.NewGuid(), userId, "Test User", DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

        _bookingRepositoryMock.Setup(r => r.GetByIdAsync(bookingId)).ReturnsAsync(booking);

        // Act
        var result = await _bookingService.CancelBookingAsync(bookingId, userId);

        // Assert
        Assert.True(result);
        _bookingRepositoryMock.Verify(r => r.DeleteAsync(bookingId), Times.Once);
    }

    [Fact]
    public async Task CancelBookingAsync_ThrowsUnauthorizedAccessException_WhenNotOwner()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var requesterId = Guid.NewGuid();
        var booking = new Booking(Guid.NewGuid(), ownerId, "Owner", DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

        _bookingRepositoryMock.Setup(r => r.GetByIdAsync(bookingId)).ReturnsAsync(booking);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _bookingService.CancelBookingAsync(bookingId, requesterId));
    }

    [Fact]
    public async Task CancelBookingAsync_ReturnsFalse_WhenBookingDoesNotExist()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        _bookingRepositoryMock.Setup(r => r.GetByIdAsync(bookingId)).ReturnsAsync((Booking?)null);

        // Act
        var result = await _bookingService.CancelBookingAsync(bookingId, Guid.NewGuid());

        // Assert
        Assert.False(result);
    }
}
