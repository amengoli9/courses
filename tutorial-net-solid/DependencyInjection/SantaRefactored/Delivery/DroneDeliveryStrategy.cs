using SantasWorkshop.Interfaces.Delivery;
using SantasWorkshop.Models;

namespace SantasWorkshop.Delivery;

/// <summary>
/// Strategia di consegna con drone
/// [L] Implementazione coerente e sostituibile
/// [I] Implementa solo le interfacce necessarie
/// </summary>
public class DroneDeliveryStrategy : IDroneDelivery
{
    public void Deliver(Toy toy)
    {
        Console.WriteLine("\n🚁 === CONSEGNA CON DRONE ===");
        Console.WriteLine($"Drone-Elfo attivato");
        Console.WriteLine($"GPS impostato su {toy.Country}");
        Console.WriteLine($"📦 Pacco lasciato alla porta");
        Console.WriteLine("Nessun biscotto ☹️");
    }

    public void ChargeBattery()
    {
        Console.WriteLine("🔋 Batterie laboratorio ricaricate");
    }

    public void UpdateGPS()
    {
        Console.WriteLine("🛰️ Satelliti GPS aggiornati");
    }
}
