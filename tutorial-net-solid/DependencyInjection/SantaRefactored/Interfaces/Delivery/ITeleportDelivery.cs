namespace SantasWorkshop.Interfaces.Delivery;

/// <summary>
/// [I] Interfaccia specifica per teletrasporto
/// Separata perché solo il teletrasporto usa magia
/// </summary>
public interface ITeleportDelivery : IDeliveryStrategy
{
    void CastSpell();
}
