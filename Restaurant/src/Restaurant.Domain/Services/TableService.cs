using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using Restaurant.Domain.Repositories;

namespace Restaurant.Domain.Services;

public class TableService : ITableService
{
    private readonly ITableRepository _repository;

    public TableService(ITableRepository repository)
    {
        _repository = repository;
    }

    public async Task<Table?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Table>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }

    public async Task<IEnumerable<Table>> GetByStatusAsync(TableStatus status, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByStatusAsync(status, cancellationToken);
    }

    public async Task<IEnumerable<Table>> GetAvailableTablesAsync(int capacity, CancellationToken cancellationToken = default)
    {
        return await _repository.GetAvailableTablesAsync(capacity, cancellationToken);
    }

    public async Task<Table> CreateAsync(Table table, CancellationToken cancellationToken = default)
    {
        return await _repository.AddAsync(table, cancellationToken);
    }

    public async Task<Table?> UpdateAsync(Guid id, Table table, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
            return null;

        existing.TableNumber = table.TableNumber;
        existing.Capacity = table.Capacity;
        existing.Status = table.Status;
        existing.Location = table.Location;
        existing.Notes = table.Notes;
        existing.UpdatedAt = DateTime.UtcNow;

        return await _repository.UpdateAsync(existing, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }
}
