using Microsoft.EntityFrameworkCore;
using Ristorante.Domain.Entities;
using Ristorante.Infrastructure.Configurations;

namespace Ristorante.Infrastructure.Data;

public class RistoranteDbContext : DbContext
{
    public RistoranteDbContext(DbContextOptions<RistoranteDbContext> options)
        : base(options)
    {
    }

    public DbSet<Table> Tables => Set<Table>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TableConfiguration());
        modelBuilder.ApplyConfiguration(new ReservationConfiguration());
    }
}
