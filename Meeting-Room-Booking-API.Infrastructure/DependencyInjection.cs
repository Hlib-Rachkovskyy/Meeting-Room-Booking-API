using Meeting_Room_Booking_API.Domain.Interfaces;
using Meeting_Room_Booking_API.Infrastructure.Data;
using Meeting_Room_Booking_API.Infrastructure.Repositories;
using Meeting_Room_Booking_API.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Meeting_Room_Booking_API.Infrastructure;

/// <summary>
/// Extension method to register all Infrastructure-layer services into the DI container.
/// Called from the WebApi project's Program.cs to keep registration logic co-located
/// with the implementations.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // EF Core InMemory provider for development / demo purposes
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("MeetingRoomBookingDb"));

        // Repositories
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Services
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
