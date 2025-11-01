using RestaurantMenu.Application.DTOs;
using RestaurantMenu.Domain.Enums;

namespace RestaurantMenu.Application.Services;

public interface IMenuItemService
{
    Task<MenuItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MenuItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<MenuItemDto>> GetByCategoryAsync(MenuCategory category, CancellationToken cancellationToken = default);
    Task<IEnumerable<MenuItemDto>> GetAvailableAsync(CancellationToken cancellationToken = default);
    Task<MenuItemDto> CreateAsync(CreateMenuItemRequest request, CancellationToken cancellationToken = default);
    Task<MenuItemDto?> UpdateAsync(Guid id, UpdateMenuItemRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
