using SantasWorkshop.Interfaces;
using SantasWorkshop.Models;

namespace SantasWorkshop.Infrastructure;

/// <summary>
/// Repository in-memory per i giocattoli
/// </summary>
public class InMemoryToyRepository : IToyRepository
{
    private readonly List<Toy> _toys = new();

    public void Add(Toy toy) => _toys.Add(toy);

    public IEnumerable<Toy> GetAll() => _toys;

    public Toy? GetByIndex(int index) => index < _toys.Count ? _toys[index] : null;

    public int Count => _toys.Count;
}
