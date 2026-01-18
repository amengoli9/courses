namespace HexagonalArchitecture.Domain.Ports;

/// <summary>
/// PORTA (Port) - Interfaccia per notifiche
/// Il dominio definisce cosa serve (inviare notifiche) ma non come (email, SMS, ecc.)
/// </summary>
public interface INotificationService
{
    Task NotifyOrderConfirmedAsync(Order order);
    Task NotifyOrderCancelledAsync(Order order);
}
