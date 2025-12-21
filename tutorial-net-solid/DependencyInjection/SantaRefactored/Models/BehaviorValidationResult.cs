namespace SantasWorkshop.Models;

/// <summary>
/// Risultato della validazione del comportamento
/// </summary>
public class BehaviorValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
}
