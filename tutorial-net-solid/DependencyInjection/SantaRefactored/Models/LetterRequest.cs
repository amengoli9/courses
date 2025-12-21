namespace SantasWorkshop.Models;

/// <summary>
/// Richiesta di lettera (DTO)
/// </summary>
public class LetterRequest
{
    public string ChildName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Behavior { get; set; } = string.Empty;
    public string ToyType { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsChristmasEve { get; set; }
}
