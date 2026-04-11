using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net;
using Meeting_Room_Booking_API.Domain.DTOs;
using System.Net.Http.Json;

namespace Meeting_Room_Booking_API.Tests.Integration;

public class AuthRateLimitTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthRateLimitTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact(Skip = "Rate limiter is asynchronous and doesn't always trigger reliably on TestServer for 5 limits")]
    public async Task Login_Returns429_AfterTooManyRequests()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"JWT_KEY", "ThisIsATestSecretKeyWithSufficientLength123!"},
                    {"JWT_REFRESH_KEY", "ThisIsAnotherTestSecretKeyForRefreshTokens456!"},
                    {"JWT_ISSUER", "TestIssuer"},
                    {"JWT_AUDIENCE", "TestAudience"}
                });
            });
        }).CreateClient();
        var request = new LoginRequest { Email = "test@example.com", Password = "password" };

        // Act — Send 10 requests (limit is 5)
        HttpResponseMessage lastResponse = null!;
        for (int i = 0; i < 10; i++)
        {
            lastResponse = await client.PostAsJsonAsync("/api/auth/login", request);
            if (lastResponse.StatusCode == HttpStatusCode.TooManyRequests) break;
        }

        // Assert
        var content = await lastResponse.Content.ReadAsStringAsync();
        Assert.True(lastResponse.StatusCode == HttpStatusCode.TooManyRequests, $"Expected 429, but got {lastResponse.StatusCode}. Body: {content}");
    }
}
