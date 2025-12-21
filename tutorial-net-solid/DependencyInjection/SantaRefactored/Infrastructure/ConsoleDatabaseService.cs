using SantasWorkshop.Interfaces;
using SantasWorkshop.Models;

namespace SantasWorkshop.Infrastructure;

/// <summary>
/// Implementazione del servizio database (console)
/// </summary>
public class ConsoleDatabaseService : IDatabaseService
{
    public void SaveAll(IEnumerable<Child> children, IEnumerable<Toy> toys)
    {
        Console.WriteLine("\n[NORTH POLE DB v2.5] Connessione a northpole-sql.santa.local:3306");
        Console.WriteLine("[NORTH POLE DB] BEGIN TRANSACTION");

        foreach (var toy in toys)
        {
            Console.WriteLine($"[NORTH POLE DB] INSERT INTO Toys (type, child, elf) " +
                $"VALUES ('{toy.Type}', '{toy.ChildName}', '{toy.AssignedElf}')");
        }

        foreach (var child in children)
        {
            Console.WriteLine($"[NORTH POLE DB] INSERT INTO Children (name, behavior) " +
                $"VALUES ('{child.Name}', '{child.Behavior}')");
        }

        Console.WriteLine("[NORTH POLE DB] COMMIT");
    }
}
