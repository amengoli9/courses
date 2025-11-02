using Microsoft.EntityFrameworkCore;
using Ristorante.Domain.Entities;
using Ristorante.Domain.Enums;
using Ristorante.Domain.Repositories;
using Ristorante.Infrastructure.Data;

namespace Ristorante.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly RistoranteDbContext _context;

    public ReservationRepository(RistoranteDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .AsNoTracking()
            .Include(r => r.Table)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .AsNoTracking()
            .Include(r => r.Table)
            .OrderByDescending(x => x.ReservationDateTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetByTableIdAsync(Guid tableId, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .AsNoTracking()
            .Include(r => r.Table)
            .Where(x => x.TableId == tableId)
            .OrderByDescending(x => x.ReservationDateTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .AsNoTracking()
            .Include(r => r.Table)
            .Where(x => x.Status == status)
            .OrderByDescending(x => x.ReservationDateTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .AsNoTracking()
            .Include(r => r.Table)
            .Where(x => x.ReservationDateTime >= startDate && x.ReservationDateTime <= endDate)
            .OrderBy(x => x.ReservationDateTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetByCustomerEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .AsNoTracking()
            .Include(r => r.Table)
            .Where(x => x.CustomerEmail == email)
            .OrderByDescending(x => x.ReservationDateTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<Reservation> AddAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        await _context.Reservations.AddAsync(reservation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return reservation;
    }

    public async Task<Reservation> UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync(cancellationToken);
        return reservation;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var reservation = await _context.Reservations.FindAsync(new object[] { id }, cancellationToken);
        if (reservation is null)
            return false;

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations.AnyAsync(x => x.Id == id, cancellationToken);
    }
}
