using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Meeting_Room_Booking_API.Domain.Interfaces;
using Meeting_Room_Booking_API.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace Meeting_Room_Booking_API.Tests.Integration;

public class StartupTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public StartupTests(WebApplicationFactory<Program> factory)
    {
        // Use a customized factory that clears environment variables/config for the test
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.Sources.Clear();
                // Add an empty config to simulate missing keys
                config.AddInMemoryCollection(new Dictionary<string, string?>());
            });
        });
    }

    [Fact]
    public void Startup_Throws_WhenJwtKeyIsMissing()
    {
        // We expect AuthService construction to fail or its methods to throw 
        // if we don't have the required secrets.
        // Since AuthService is injected into the controller, let's try getting it.

        using var scope = _factory.Services.CreateScope();
        
        // This should throw because AuthService.GetConfigValue will (eventually) throw
        // if the key is missing and fallback is removed.
        var ex = Assert.Throws<InvalidOperationException>(() => 
            scope.ServiceProvider.GetRequiredService<IAuthService>());
        
        Assert.Contains("JWT_KEY", ex.Message);
    }
}
