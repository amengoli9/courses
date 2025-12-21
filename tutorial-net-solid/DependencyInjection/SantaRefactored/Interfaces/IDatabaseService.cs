using SantasWorkshop.Models;

namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Servizio di persistenza su database
/// [D] Astrazione per la persistenza
/// </summary>
public interface IDatabaseService
{
    void SaveAll(IEnumerable<Child> children, IEnumerable<Toy> toys);
}
