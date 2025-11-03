using Restaurant.Domain.Enums;

namespace Restaurant.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid TableId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public int NumberOfGuests { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public ReservationStatus Status { get; set; }
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public Table Table { get; set; } = null!;

    public Reservation()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Status = ReservationStatus.Pending;
    }
}
