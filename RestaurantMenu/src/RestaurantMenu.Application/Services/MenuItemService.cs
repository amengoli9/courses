using RestaurantMenu.Application.DTOs;
using RestaurantMenu.Domain.Entities;
using RestaurantMenu.Domain.Enums;
using RestaurantMenu.Domain.Repositories;

namespace RestaurantMenu.Application.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _repository;

    public MenuItemService(IMenuItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<MenuItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var menuItem = await _repository.GetByIdAsync(id, cancellationToken);
        return menuItem is not null ? MapToDto(menuItem) : null;
    }

    public async Task<IEnumerable<MenuItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var menuItems = await _repository.GetAllAsync(cancellationToken);
        return menuItems.Select(MapToDto);
    }

    public async Task<IEnumerable<MenuItemDto>> GetByCategoryAsync(MenuCategory category, CancellationToken cancellationToken = default)
    {
        var menuItems = await _repository.GetByCategoryAsync(category, cancellationToken);
        return menuItems.Select(MapToDto);
    }

    public async Task<IEnumerable<MenuItemDto>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        var menuItems = await _repository.GetAvailableAsync(cancellationToken);
        return menuItems.Select(MapToDto);
    }

    public async Task<MenuItemDto> CreateAsync(CreateMenuItemRequest request, CancellationToken cancellationToken = default)
    {
        var menuItem = new MenuItem
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            IsAvailable = request.IsAvailable,
            Allergens = request.Allergens ?? new List<string>(),
            ImageUrl = request.ImageUrl,
            PreparationTimeMinutes = request.PreparationTimeMinutes
        };

        var created = await _repository.AddAsync(menuItem, cancellationToken);
        return MapToDto(created);
    }

    public async Task<MenuItemDto?> UpdateAsync(Guid id, UpdateMenuItemRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
            return null;

        existing.Name = request.Name;
        existing.Description = request.Description;
        existing.Price = request.Price;
        existing.Category = request.Category;
        existing.IsAvailable = request.IsAvailable;
        existing.Allergens = request.Allergens ?? new List<string>();
        existing.ImageUrl = request.ImageUrl;
        existing.PreparationTimeMinutes = request.PreparationTimeMinutes;
        existing.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(existing, cancellationToken);
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }

    private static MenuItemDto MapToDto(MenuItem menuItem)
    {
        return new MenuItemDto(
            menuItem.Id,
            menuItem.Name,
            menuItem.Description,
            menuItem.Price,
            menuItem.Category,
            menuItem.IsAvailable,
            menuItem.Allergens,
            menuItem.ImageUrl,
            menuItem.PreparationTimeMinutes,
            menuItem.CreatedAt,
            menuItem.UpdatedAt
        );
    }
}
