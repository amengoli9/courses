namespace SantasWorkshop.Models;

/// <summary>
/// Dati del bambino
/// </summary>
public class Child
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Behavior { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string RequestedToy { get; set; } = string.Empty;
}
