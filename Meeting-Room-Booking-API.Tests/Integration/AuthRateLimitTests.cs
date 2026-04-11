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

    [Fact]
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
        var request = new LoginRequest("test@example.com", "password");

        // Act — Send 6 requests (limit is 5)
        HttpResponseMessage lastResponse = null!;
        for (int i = 0; i < 6; i++)
        {
            lastResponse = await client.PostAsJsonAsync("/api/auth/login", request);
        }

        // Assert
        var content = await lastResponse.Content.ReadAsStringAsync();
        Assert.True(lastResponse.StatusCode == HttpStatusCode.TooManyRequests, $"Expected 429, but got {lastResponse.StatusCode}. Body: {content}");
    }
}
