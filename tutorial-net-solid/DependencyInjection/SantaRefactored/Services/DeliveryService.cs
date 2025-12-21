using SantasWorkshop.Interfaces;
using SantasWorkshop.Interfaces.Delivery;

namespace SantasWorkshop.Services;

/// <summary>
/// Implementazione del servizio di consegna
/// [S] Una sola responsabilità: gestire le consegne
/// [D] Dipende da astrazioni (factory e repository)
/// </summary>
public class DeliveryService : IDeliveryService
{
    private readonly IToyRepository _toyRepository;
    private readonly IDeliveryStrategyFactory _deliveryStrategyFactory;

    public DeliveryService(IToyRepository toyRepository, IDeliveryStrategyFactory deliveryStrategyFactory)
    {
        _toyRepository = toyRepository;
        _deliveryStrategyFactory = deliveryStrategyFactory;
    }

    public void DeliverPresent(string deliveryType, int toyIndex)
    {
        var toy = _toyRepository.GetByIndex(toyIndex);
        if (toy == null)
        {
            Console.WriteLine("❌ Giocattolo non trovato!");
            return;
        }

        var strategy = _deliveryStrategyFactory.Create(deliveryType);
        strategy.Deliver(toy);
    }
}
