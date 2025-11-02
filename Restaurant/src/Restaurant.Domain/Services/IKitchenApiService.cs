namespace Restaurant.Domain.Services;

public interface IKitchenApiService
{
    Task<IEnumerable<MenuItemDto>> GetMenuAsync(CancellationToken cancellationToken = default);
    Task<MenuItemDto?> GetMenuItemByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MenuItemDto>> GetAvailableMenuAsync(CancellationToken cancellationToken = default);
}

public record MenuItemDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Category,
    bool IsAvailable,
    bool IsVegetarian,
    bool IsVegan,
    bool IsGlutenFree,
    List<string> Allergens,
    int PreparationTimeMinutes
);
