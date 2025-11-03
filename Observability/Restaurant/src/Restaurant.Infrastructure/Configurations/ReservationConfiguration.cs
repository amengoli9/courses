using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Infrastructure.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("reservations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.TableId)
            .HasColumnName("table_id")
            .IsRequired();

        builder.Property(x => x.CustomerName)
            .HasColumnName("customer_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CustomerEmail)
            .HasColumnName("customer_email")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.CustomerPhone)
            .HasColumnName("customer_phone")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.NumberOfGuests)
            .HasColumnName("number_of_guests")
            .IsRequired();

        builder.Property(x => x.ReservationDateTime)
            .HasColumnName("reservation_date_time")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.SpecialRequests)
            .HasColumnName("special_requests")
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        // Indexes
        builder.HasIndex(x => x.TableId)
            .HasDatabaseName("idx_reservations_table_id");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("idx_reservations_status");

        builder.HasIndex(x => x.ReservationDateTime)
            .HasDatabaseName("idx_reservations_date_time");

        builder.HasIndex(x => x.CustomerEmail)
            .HasDatabaseName("idx_reservations_customer_email");
    }
}
