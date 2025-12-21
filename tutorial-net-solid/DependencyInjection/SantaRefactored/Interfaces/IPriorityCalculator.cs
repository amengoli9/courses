namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Calcola la priorità di consegna
/// </summary>
public interface IPriorityCalculator
{
    int CalculatePriority(bool isChristmasEve, int age, string country);
}
