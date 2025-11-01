using RestaurantMenu.Domain.Enums;

namespace RestaurantMenu.Application.DTOs;

public record MenuItemDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    MenuCategory Category,
    bool IsAvailable,
    List<string> Allergens,
    string? ImageUrl,
    int PreparationTimeMinutes,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
