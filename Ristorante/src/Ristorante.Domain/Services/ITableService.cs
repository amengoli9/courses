using Ristorante.Domain.Entities;
using Ristorante.Domain.Enums;

namespace Ristorante.Domain.Services;

public interface ITableService
{
    Task<Table?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Table>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Table>> GetByStatusAsync(TableStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Table>> GetAvailableTablesAsync(int capacity, CancellationToken cancellationToken = default);
    Task<Table> CreateAsync(Table table, CancellationToken cancellationToken = default);
    Task<Table?> UpdateAsync(Guid id, Table table, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
