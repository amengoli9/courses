using SantasWorkshop.Models;

namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Repository per i bambini
/// [D] Astrazione per la persistenza
/// </summary>
public interface IChildRepository
{
    void Add(Child child);
    IEnumerable<Child> GetAll();
}
