using Meeting_Room_Booking_API.Domain.Entities;
using Xunit;

namespace Meeting_Room_Booking_API.Tests.Domain;

public class RoomTests
{
    [Fact]
    public void Constructor_Succeeds_WithValidData()
    {
        // Arrange
        var name = "Conference Room";
        var location = "Floor 2";
        var capacity = 10;

        // Act
        var room = new Room(name, location, capacity);

        // Assert
        Assert.Equal(name, room.Name);
        Assert.Equal(location, room.Location);
        Assert.Equal(capacity, room.Capacity);
        Assert.NotEqual(Guid.Empty, room.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_ThrowsArgumentException_WhenNameIsInvalid(string? name)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Room(name!, "Location", 5));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_ThrowsArgumentOutOfRangeException_WhenCapacityIsInvalid(int capacity)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Room("Room Name", "Location", capacity));
    }
}
