using Microsoft.EntityFrameworkCore;
using Ristorante.Domain.Entities;
using Ristorante.Domain.Enums;
using Ristorante.Domain.Repositories;
using Ristorante.Infrastructure.Data;

namespace Ristorante.Infrastructure.Repositories;

public class TableRepository : ITableRepository
{
    private readonly RistoranteDbContext _context;

    public TableRepository(RistoranteDbContext context)
    {
        _context = context;
    }

    public async Task<Table?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tables
            .AsNoTracking()
            .Include(t => t.Reservations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Table>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tables
            .AsNoTracking()
            .Include(t => t.Reservations)
            .OrderBy(x => x.TableNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Table>> GetByStatusAsync(TableStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Tables
            .AsNoTracking()
            .Include(t => t.Reservations)
            .Where(x => x.Status == status)
            .OrderBy(x => x.TableNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Table>> GetAvailableTablesAsync(int capacity, CancellationToken cancellationToken = default)
    {
        return await _context.Tables
            .AsNoTracking()
            .Include(t => t.Reservations)
            .Where(x => x.Status == TableStatus.Available && x.Capacity >= capacity)
            .OrderBy(x => x.Capacity)
            .ToListAsync(cancellationToken);
    }

    public async Task<Table> AddAsync(Table table, CancellationToken cancellationToken = default)
    {
        await _context.Tables.AddAsync(table, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return table;
    }

    public async Task<Table> UpdateAsync(Table table, CancellationToken cancellationToken = default)
    {
        _context.Tables.Update(table);
        await _context.SaveChangesAsync(cancellationToken);
        return table;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var table = await _context.Tables.FindAsync(new object[] { id }, cancellationToken);
        if (table is null)
            return false;

        _context.Tables.Remove(table);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tables.AnyAsync(x => x.Id == id, cancellationToken);
    }
}
