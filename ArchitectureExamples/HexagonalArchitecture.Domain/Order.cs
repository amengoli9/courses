namespace HexagonalArchitecture.Domain;

/// <summary>
/// Entità del dominio - rappresenta un ordine
/// Nel dominio esagonale, le entità sono al centro e non dipendono da nulla
/// </summary>
public class Order
{
    public Guid Id { get; private set; }
    public string CustomerName { get; private set; }
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Order(string customerName, decimal totalAmount)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("Customer name cannot be empty", nameof(customerName));

        if (totalAmount <= 0)
            throw new ArgumentException("Total amount must be positive", nameof(totalAmount));

        Id = Guid.NewGuid();
        CustomerName = customerName;
        TotalAmount = totalAmount;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be confirmed");

        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Delivered orders cannot be cancelled");

        Status = OrderStatus.Cancelled;
    }
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Delivered,
    Cancelled
}
