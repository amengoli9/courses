using SantasWorkshop.Interfaces.Delivery;
using SantasWorkshop.Models;

namespace SantasWorkshop.Delivery;

/// <summary>
/// Strategia di fallback per consegne generiche
/// </summary>
public class PostalDeliveryStrategy : IDeliveryStrategy
{
    public void Deliver(Toy toy)
    {
        Console.WriteLine($"📬 Regalo spedito via posta a {toy.ChildName}");
    }
}
