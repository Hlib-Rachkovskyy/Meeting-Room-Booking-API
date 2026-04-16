using System.Net;
using System.Text.Json;
using Meeting_Room_Booking_API.Domain.Exceptions;
using Meeting_Room_Booking_API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Meeting_Room_Booking_API.Tests.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    private static TestServer BuildServerThatThrows(Exception ex)
    {
        var builder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddLogging(b => b.SetMinimumLevel(LogLevel.None));
            })
            .Configure(app =>
            {
                app.UseMiddleware<ExceptionHandlingMiddleware>();
                app.Run(_ => throw ex);
            });

        return new TestServer(builder);
    }

    [Fact]
    public async Task UnhandledException_Returns500_WithGenericMessage_NotLeakingExceptionDetails()
    {
        // Arrange
        const string secretDetail = "Super sensitive DB connection string leaked";
        using var server = BuildServerThatThrows(new Exception(secretDetail));
        using var client = server.CreateClient();

        // Act
        var response = await client.GetAsync("/any");
        var body = await response.Content.ReadAsStringAsync();

        // Assert — status is 500
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        // Assert — the raw exception message must NOT appear in the response body
        Assert.DoesNotContain(secretDetail, body, StringComparison.OrdinalIgnoreCase);

        // Assert — body contains a generic safe message
        Assert.Contains("unexpected error", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ArgumentException_Returns400_WithExceptionMessage()
    {
        // Arrange
        const string validationMessage = "RoomId cannot be empty";
        using var server = BuildServerThatThrows(new ArgumentException(validationMessage));
        using var client = server.CreateClient();

        // Act
        var response = await client.GetAsync("/any");
        var body = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(validationMessage, body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task UnauthorizedAccessException_Returns401()
    {
        using var server = BuildServerThatThrows(new UnauthorizedAccessException("Not allowed"));
        using var client = server.CreateClient();

        var response = await client.GetAsync("/any");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task KeyNotFoundException_Returns404()
    {
        using var server = BuildServerThatThrows(new KeyNotFoundException("Not found"));
        using var client = server.CreateClient();

        var response = await client.GetAsync("/any");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ConflictException_Returns409()
    {
        // Arrange
        const string message = "Room already booked";
        using var server = BuildServerThatThrows(new ConflictException(message));
        using var client = server.CreateClient();

        // Act
        var response = await client.GetAsync("/any");
        var body = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Contains(message, body, StringComparison.OrdinalIgnoreCase);
    }
}
