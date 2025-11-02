using Kitchen.Domain.Enums;

namespace Kitchen.Domain.Entities;

public class MenuItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public MenuCategory Category { get; set; }
    public bool IsAvailable { get; set; }
    public List<string> Allergens { get; set; } = new();
    public string? ImageUrl { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public MenuItem()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsAvailable = true;
    }
}
