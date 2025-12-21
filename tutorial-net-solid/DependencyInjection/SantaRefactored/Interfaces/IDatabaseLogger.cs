namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Logger per il database
/// [D] Astrazione per la persistenza
/// </summary>
public interface IDatabaseLogger
{
    void LogProduction(string childName, string toyType, string assignedElf);
}
