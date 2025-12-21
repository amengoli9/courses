using SantasWorkshop.Models;

namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Generatore di report
/// [D] Astrazione per il reporting
/// </summary>
public interface IReportGenerator
{
    void GenerateReport(IEnumerable<Child> children, IEnumerable<Toy> toys, int elfEnergy, int reindeerCount);
}
