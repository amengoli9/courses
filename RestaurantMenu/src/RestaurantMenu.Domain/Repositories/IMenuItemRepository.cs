using RestaurantMenu.Domain.Entities;
using RestaurantMenu.Domain.Enums;

namespace RestaurantMenu.Domain.Repositories;

public interface IMenuItemRepository
{
    Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<MenuItem>> GetByCategoryAsync(MenuCategory category, CancellationToken cancellationToken = default);
    Task<IEnumerable<MenuItem>> GetAvailableAsync(CancellationToken cancellationToken = default);
    Task<MenuItem> AddAsync(MenuItem menuItem, CancellationToken cancellationToken = default);
    Task<MenuItem> UpdateAsync(MenuItem menuItem, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
