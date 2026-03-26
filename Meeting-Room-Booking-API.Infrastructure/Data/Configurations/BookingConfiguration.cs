using Meeting_Room_Booking_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meeting_Room_Booking_API.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.BookedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.StartTime)
            .IsRequired();

        builder.Property(b => b.EndTime)
            .IsRequired();

        // Foreign key to Room is configured in RoomConfiguration
        // through the navigation property if we want to follow the aggregate root,
        // but it's safe to define it here if needed as well.
        // It's already handled by the relationship in RoomConfiguration.
    }
}
