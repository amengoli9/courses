using SantasWorkshop.Models;

namespace SantasWorkshop.Interfaces.Delivery;

/// <summary>
/// [O][L] Strategia base per le consegne
/// Ogni strategia ha un comportamento coerente e sostituibile
/// </summary>
public interface IDeliveryStrategy
{
    void Deliver(Toy toy);
}
