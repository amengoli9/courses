using SantasWorkshop.Interfaces;
using SantasWorkshop.Models;

namespace SantasWorkshop.Infrastructure;

/// <summary>
/// Implementazione del generatore di report (console)
/// </summary>
public class ConsoleReportGenerator : IReportGenerator
{
    public void GenerateReport(IEnumerable<Child> children, IEnumerable<Toy> toys, int elfEnergy, int reindeerCount)
    {
        var childList = children.ToList();
        var toyList = toys.ToList();

        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("🎄 REPORT DI NATALE - WORKSHOP DI BABBO NATALE 🎅");
        Console.WriteLine(new string('=', 60));
        Console.WriteLine($"📊 Totale bambini: {childList.Count}");
        Console.WriteLine($"🎁 Totale giocattoli prodotti: {toyList.Count}");
        Console.WriteLine($"⚡ Energia elfi rimanente: {elfEnergy}%");
        Console.WriteLine($"🦌 Renne disponibili: {reindeerCount}");

        Console.WriteLine("\n📈 STATISTICHE PER PAESE:");
        var byCountry = toyList.GroupBy(t => t.Country);
        foreach (var group in byCountry)
        {
            Console.WriteLine($"  {group.Key}: {group.Count()} regali");
        }

        Console.WriteLine("\n🎮 GIOCATTOLI PIÙ RICHIESTI:");
        var byType = toyList.GroupBy(t => t.Type);
        foreach (var group in byType.OrderByDescending(g => g.Count()).Take(3))
        {
            Console.WriteLine($"  {group.Key}: {group.Count()} richieste");
        }

        Console.WriteLine("\n👼 COMPORTAMENTO BAMBINI:");
        var goodKids = childList.Count(c => c.Behavior == "Buono");
        var naughtyKids = childList.Count(c => c.Behavior == "Cattivo");
        Console.WriteLine($"  😇 Buoni: {goodKids}");
        Console.WriteLine($"  😈 Cattivi: {naughtyKids}");

        Console.WriteLine(new string('=', 60) + "\n");
    }
}
