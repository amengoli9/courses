namespace SantasWorkshop.Models;

/// <summary>
/// Configurazione per la produzione di un giocattolo
/// </summary>
public class ToyProductionConfig
{
    public int ProductionTime { get; set; }
    public int MaterialsNeeded { get; set; }
    public string Description { get; set; } = string.Empty;
}
