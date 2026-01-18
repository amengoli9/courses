using HexagonalArchitecture.Domain;
using HexagonalArchitecture.Domain.Ports;

namespace HexagonalArchitecture.Infrastructure.Adapters;

/// <summary>
/// ADAPTER - Implementazione concreta della porta INotificationService
/// Questo adapter stampa le notifiche sulla console
/// Potrebbe essere facilmente sostituito con un adapter per Email, SMS, ecc.
/// </summary>
public class ConsoleNotificationService : INotificationService
{
    public Task NotifyOrderConfirmedAsync(Order order)
    {
        Console.WriteLine($"[NOTIFICATION] Order {order.Id} confirmed for {order.CustomerName}");
        return Task.CompletedTask;
    }

    public Task NotifyOrderCancelledAsync(Order order)
    {
        Console.WriteLine($"[NOTIFICATION] Order {order.Id} cancelled for {order.CustomerName}");
        return Task.CompletedTask;
    }
}
