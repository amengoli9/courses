using SantasWorkshop.Interfaces.Delivery;
using SantasWorkshop.Models;

namespace SantasWorkshop.Delivery;

/// <summary>
/// Strategia di consegna con teletrasporto
/// [L] Implementazione coerente e sostituibile
/// [I] Implementa solo le interfacce necessarie
/// </summary>
public class TeleportDeliveryStrategy : ITeleportDelivery
{
    public void Deliver(Toy toy)
    {
        Console.WriteLine("\n✨ === TELETRASPORTO MAGICO ===");
        Console.WriteLine("⭐ Magia natalizia attivata!");
        Console.WriteLine($"💫 *POOF* Regalo appare sotto l'albero di {toy.ChildName}!");
        Console.WriteLine("Consegna istantanea!");
    }

    public void CastSpell()
    {
        Console.WriteLine("🪄 Incantesimo di Natale lanciato!");
    }
}
