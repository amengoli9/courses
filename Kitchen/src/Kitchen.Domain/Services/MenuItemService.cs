using Kitchen.Domain.Entities;
using Kitchen.Domain.Enums;
using Kitchen.Domain.Repositories;

namespace Kitchen.Domain.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _repository;

    public MenuItemService(IMenuItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }

    public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(MenuCategory category, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByCategoryAsync(category, cancellationToken);
    }

    public async Task<IEnumerable<MenuItem>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAvailableAsync(cancellationToken);
    }

    public async Task<MenuItem> CreateAsync(MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        return await _repository.AddAsync(menuItem, cancellationToken);
    }

    public async Task<MenuItem?> UpdateAsync(Guid id, MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
            return null;

        existing.Name = menuItem.Name;
        existing.Description = menuItem.Description;
        existing.Price = menuItem.Price;
        existing.Category = menuItem.Category;
        existing.IsAvailable = menuItem.IsAvailable;
        existing.Allergens = menuItem.Allergens;
        existing.ImageUrl = menuItem.ImageUrl;
        existing.PreparationTimeMinutes = menuItem.PreparationTimeMinutes;
        existing.UpdatedAt = DateTime.UtcNow;

        return await _repository.UpdateAsync(existing, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }
}
