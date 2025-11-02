using Ristorante.Domain.Enums;

namespace Ristorante.Domain.Entities;

public class Table
{
    public Guid Id { get; set; }
    public int TableNumber { get; set; }
    public int Capacity { get; set; }
    public TableStatus Status { get; set; }
    public string Location { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public Table()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Status = TableStatus.Available;
    }
}
