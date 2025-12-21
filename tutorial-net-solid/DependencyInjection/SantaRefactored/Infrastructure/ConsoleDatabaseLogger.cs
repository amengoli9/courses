using SantasWorkshop.Interfaces;

namespace SantasWorkshop.Infrastructure;

/// <summary>
/// Implementazione del logger database (console)
/// </summary>
public class ConsoleDatabaseLogger : IDatabaseLogger
{
    public void LogProduction(string childName, string toyType, string assignedElf)
    {
        Console.WriteLine($"[NorthPoleDB] INSERT INTO Productions (child, toy, elf) VALUES ('{childName}', '{toyType}', '{assignedElf}')");
        Console.WriteLine($"[LOG FILE] {DateTime.Now}: Produzione avviata per {childName}");
    }
}
