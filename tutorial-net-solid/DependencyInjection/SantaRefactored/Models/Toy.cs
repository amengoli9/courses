namespace SantasWorkshop.Models;

/// <summary>
/// Dati del giocattolo
/// </summary>
public class Toy
{
    public string Type { get; set; } = string.Empty;
    public string ChildName { get; set; } = string.Empty;
    public int ProductionTime { get; set; }
    public int Priority { get; set; }
    public string AssignedElf { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}
