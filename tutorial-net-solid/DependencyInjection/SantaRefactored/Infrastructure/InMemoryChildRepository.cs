using SantasWorkshop.Interfaces;
using SantasWorkshop.Models;

namespace SantasWorkshop.Infrastructure;

/// <summary>
/// Repository in-memory per i bambini
/// </summary>
public class InMemoryChildRepository : IChildRepository
{
    private readonly List<Child> _children = new();

    public void Add(Child child) => _children.Add(child);

    public IEnumerable<Child> GetAll() => _children;
}
