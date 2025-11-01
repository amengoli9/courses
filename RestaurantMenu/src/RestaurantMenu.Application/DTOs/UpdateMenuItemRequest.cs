using RestaurantMenu.Domain.Enums;

namespace RestaurantMenu.Application.DTOs;

public record UpdateMenuItemRequest(
    string Name,
    string Description,
    decimal Price,
    MenuCategory Category,
    bool IsAvailable,
    List<string> Allergens,
    string? ImageUrl,
    int PreparationTimeMinutes
);
