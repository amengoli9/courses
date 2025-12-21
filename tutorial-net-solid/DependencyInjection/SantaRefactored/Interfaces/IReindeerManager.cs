namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Gestisce lo stato delle renne (condiviso tra servizi)
/// </summary>
public interface IReindeerManager
{
    int Count { get; }
    void UseReindeer();
    void RestoreReindeer();
}
