using Kitchen.Domain.Entities;
using Kitchen.Domain.Enums;

namespace Kitchen.Domain.Services;

public interface IMenuItemService
{
    Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<MenuItem>> GetByCategoryAsync(MenuCategory category, CancellationToken cancellationToken = default);
    Task<IEnumerable<MenuItem>> GetAvailableAsync(CancellationToken cancellationToken = default);
    Task<MenuItem> CreateAsync(MenuItem menuItem, CancellationToken cancellationToken = default);
    Task<MenuItem?> UpdateAsync(Guid id, MenuItem menuItem, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
