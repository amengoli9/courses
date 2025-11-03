using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;

namespace Restaurant.Domain.Repositories;

public interface ITableRepository
{
    Task<Table?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Table>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Table>> GetByStatusAsync(TableStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Table>> GetAvailableTablesAsync(int capacity, CancellationToken cancellationToken = default);
    Task<Table> AddAsync(Table table, CancellationToken cancellationToken = default);
    Task<Table> UpdateAsync(Table table, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
