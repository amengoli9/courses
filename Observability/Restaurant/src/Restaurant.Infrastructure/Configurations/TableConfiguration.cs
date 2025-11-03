using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;

namespace Restaurant.Infrastructure.Configurations;

public class TableConfiguration : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.ToTable("tables");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.TableNumber)
            .HasColumnName("table_number")
            .IsRequired();

        builder.Property(x => x.Capacity)
            .HasColumnName("capacity")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Location)
            .HasColumnName("location")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasColumnName("notes")
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        // Indexes
        builder.HasIndex(x => x.TableNumber)
            .HasDatabaseName("idx_tables_table_number")
            .IsUnique();

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("idx_tables_status");

        // Relationships
        builder.HasMany(x => x.Reservations)
            .WithOne(x => x.Table)
            .HasForeignKey(x => x.TableId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        builder.HasData(
            new Table
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                TableNumber = 1,
                Capacity = 4,
                Status = TableStatus.Available,
                Location = "Sala principale - Finestra",
                CreatedAt = DateTime.UtcNow
            },
            new Table
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                TableNumber = 2,
                Capacity = 2,
                Status = TableStatus.Available,
                Location = "Sala principale - Centro",
                CreatedAt = DateTime.UtcNow
            },
            new Table
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                TableNumber = 3,
                Capacity = 6,
                Status = TableStatus.Available,
                Location = "Sala privata",
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
