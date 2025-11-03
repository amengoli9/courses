using Microsoft.EntityFrameworkCore;
using Kitchen.Domain.Entities;
using Kitchen.Infrastructure.Configurations;

namespace Kitchen.Infrastructure.Data;

public class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options)
    {
    }

    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new MenuItemConfiguration());
    }
}
