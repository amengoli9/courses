namespace SantasWorkshop.Interfaces.Delivery;

/// <summary>
/// [I] Interfaccia specifica per consegne con slitta
/// Separata perché solo la slitta usa le renne
/// </summary>
public interface ISleighDelivery : IDeliveryStrategy
{
    void FeedReindeer();
    void PolishSleigh();
}
