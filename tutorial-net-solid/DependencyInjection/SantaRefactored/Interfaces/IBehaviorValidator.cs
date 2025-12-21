using SantasWorkshop.Models;

namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Valida il comportamento del bambino
/// </summary>
public interface IBehaviorValidator
{
    BehaviorValidationResult Validate(string behavior, int age);
}
