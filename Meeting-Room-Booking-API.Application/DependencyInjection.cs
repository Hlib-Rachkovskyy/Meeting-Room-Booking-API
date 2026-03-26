using Meeting_Room_Booking_API.Application.Services;
using Meeting_Room_Booking_API.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Meeting_Room_Booking_API.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBookingService, BookingService>();
        return services;
    }
}
