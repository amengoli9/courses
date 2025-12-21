using SantasWorkshop.Interfaces;
using SantasWorkshop.Models;

namespace SantasWorkshop.Infrastructure;

/// <summary>
/// Implementazione del servizio di notifica via email (console)
/// </summary>
public class ConsoleNotificationService : INotificationService
{
    public void SendProductionNotification(Child child, Toy toy, string assignedElf)
    {
        Console.WriteLine("\n📧 === NOTIFICA VIA EMAIL ===");
        Console.WriteLine($"A: santa@northpole.com");
        Console.WriteLine($"Oggetto: Nuovo ordine #{toy.Priority}");
        Console.WriteLine($"Bambino: {child.Name} ({child.Age} anni)");
        Console.WriteLine($"Giocattolo: {toy.Type}");
        Console.WriteLine($"Elfo: {assignedElf}");
        Console.WriteLine("============================\n");
    }
}
