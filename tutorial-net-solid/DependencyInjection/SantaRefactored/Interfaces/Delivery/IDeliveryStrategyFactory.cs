namespace SantasWorkshop.Interfaces.Delivery;

/// <summary>
/// [O] Factory per creare strategie di consegna
/// Permette di aggiungere nuove strategie senza modificare il codice esistente
/// </summary>
public interface IDeliveryStrategyFactory
{
    IDeliveryStrategy Create(string deliveryType);
}
