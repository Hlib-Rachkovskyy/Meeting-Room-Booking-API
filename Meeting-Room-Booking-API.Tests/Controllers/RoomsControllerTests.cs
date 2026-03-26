using Meeting_Room_Booking_API.Controllers;
using Meeting_Room_Booking_API.Domain.Entities;
using Meeting_Room_Booking_API.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Meeting_Room_Booking_API.Tests.Controllers;

public class RoomsControllerTests
{
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly RoomsController _controller;

    public RoomsControllerTests()
    {
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _controller = new RoomsController(_roomRepositoryMock.Object);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenRoomExists()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var room = new Room("Test Room", "Location A", 10);
        
        _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId, true))
            .ReturnsAsync(room);

        // Act
        var result = await _controller.GetById(roomId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedRoom = Assert.IsType<Room>(okResult.Value);
        Assert.Equal(room.Name, returnedRoom.Name);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenRoomDoesNotExist()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId, true))
            .ReturnsAsync((Room?)null);

        // Act
        var result = await _controller.GetById(roomId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
