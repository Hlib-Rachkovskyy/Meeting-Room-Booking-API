using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Meeting_Room_Booking_API.Tests.Integration;

public class GlobalRateLimitTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GlobalRateLimitTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task NonAuthEndpoint_Returns429_After100Requests()
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
        
        // Use an endpoint that does NOT have [EnableRateLimiting("AuthLimit")]
        // e.g., the swagger.json endpoint or a non-authenticated ping.
        // We will just hit a nonexistent global endpoint /api/rooms since even a 401/404 
        // implies the request went through the global limiter pipeline.
        var endpoint = "/api/rooms";

        // Act — Send 101 requests (Global limit is 100)
        HttpResponseMessage lastResponse = null!;
        for (int i = 0; i < 101; i++)
        {
            lastResponse = await client.GetAsync(endpoint);
        }

        // Assert
        Assert.Equal((HttpStatusCode)429, lastResponse.StatusCode);
    }
}
