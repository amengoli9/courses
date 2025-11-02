using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Kitchen.Domain.Entities;
using Kitchen.Domain.Enums;

namespace Kitchen.Infrastructure.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("menu_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Price)
            .HasColumnName("price")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(x => x.Category)
            .HasColumnName("category")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.IsAvailable)
            .HasColumnName("is_available")
            .IsRequired();

        builder.Property(x => x.Allergens)
            .HasColumnName("allergens")
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(x => x.ImageUrl)
            .HasColumnName("image_url")
            .HasMaxLength(500);

        builder.Property(x => x.PreparationTimeMinutes)
            .HasColumnName("preparation_time_minutes")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        // Indexes
        builder.HasIndex(x => x.Category)
            .HasDatabaseName("idx_menu_items_category");

        builder.HasIndex(x => x.IsAvailable)
            .HasDatabaseName("idx_menu_items_is_available");

        // Seed data
        builder.HasData(
            new MenuItem
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Bruschetta al Pomodoro",
                Description = "Pane tostato con pomodori freschi, basilico e olio d'oliva",
                Price = 8.50m,
                Category = MenuCategory.Antipasti,
                IsAvailable = true,
                Allergens = new List<string> { "Glutine" },
                PreparationTimeMinutes = 10,
                CreatedAt = DateTime.UtcNow
            },
            new MenuItem
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Spaghetti alla Carbonara",
                Description = "Spaghetti con guanciale, uova, pecorino romano e pepe nero",
                Price = 14.00m,
                Category = MenuCategory.PrimiPiatti,
                IsAvailable = true,
                Allergens = new List<string> { "Glutine", "Uova", "Latticini" },
                PreparationTimeMinutes = 20,
                CreatedAt = DateTime.UtcNow
            },
            new MenuItem
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Tagliata di Manzo",
                Description = "Filetto di manzo tagliato, rucola e grana",
                Price = 22.00m,
                Category = MenuCategory.SecondiPiatti,
                IsAvailable = true,
                Allergens = new List<string> { "Latticini" },
                PreparationTimeMinutes = 25,
                CreatedAt = DateTime.UtcNow
            },
            new MenuItem
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Tiramisù",
                Description = "Dolce tradizionale con savoiardi, mascarpone, caffè e cacao",
                Price = 7.50m,
                Category = MenuCategory.Dolci,
                IsAvailable = true,
                Allergens = new List<string> { "Glutine", "Uova", "Latticini" },
                PreparationTimeMinutes = 5,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
