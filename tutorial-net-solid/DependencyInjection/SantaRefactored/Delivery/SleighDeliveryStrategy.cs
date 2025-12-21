using SantasWorkshop.Interfaces;
using SantasWorkshop.Interfaces.Delivery;
using SantasWorkshop.Models;

namespace SantasWorkshop.Delivery;

/// <summary>
/// Strategia di consegna con slitta
/// [L] Implementazione coerente e sostituibile
/// [I] Implementa solo le interfacce necessarie
/// </summary>
public class SleighDeliveryStrategy : ISleighDelivery
{
    private readonly IReindeerManager _reindeerManager;

    public SleighDeliveryStrategy(IReindeerManager reindeerManager)
    {
        _reindeerManager = reindeerManager;
    }

    public void Deliver(Toy toy)
    {
        Console.WriteLine("\n🎅 === CONSEGNA CON SLITTA ===");
        Console.WriteLine($"Babbo Natale sale sulla slitta con {_reindeerManager.Count} renne");
        Console.WriteLine("🦌 Rudolf guida con il naso rosso!");
        Console.WriteLine($"🌟 Volo magico verso {toy.Country}");
        Console.WriteLine($"🏠 Scende dal camino");
        Console.WriteLine($"🎁 Deposita il {toy.Type} sotto l'albero");
        Console.WriteLine($"🍪 Mangia biscotti e beve latte");
        Console.WriteLine("✨ Torna al Polo Nord volando");
        _reindeerManager.UseReindeer();
    }

    public void FeedReindeer()
    {
        Console.WriteLine("🥕 Renne nutrite con carote magiche");
    }

    public void PolishSleigh()
    {
        Console.WriteLine("✨ Slitta lucidata a specchio!");
    }
}
