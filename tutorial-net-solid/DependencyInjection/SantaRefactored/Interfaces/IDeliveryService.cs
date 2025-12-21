namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Gestisce le consegne
/// </summary>
public interface IDeliveryService
{
    void DeliverPresent(string deliveryType, int toyIndex);
}
