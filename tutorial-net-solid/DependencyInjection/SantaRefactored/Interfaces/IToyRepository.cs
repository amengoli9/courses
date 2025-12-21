using SantasWorkshop.Models;

namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Repository per i giocattoli
/// [D] Astrazione per la persistenza
/// </summary>
public interface IToyRepository
{
    void Add(Toy toy);
    IEnumerable<Toy> GetAll();
    Toy? GetByIndex(int index);
    int Count { get; }
}
