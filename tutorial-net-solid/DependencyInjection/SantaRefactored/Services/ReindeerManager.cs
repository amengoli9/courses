using SantasWorkshop.Interfaces;

namespace SantasWorkshop.Services;

/// <summary>
/// Implementazione del gestore renne (stato condiviso)
/// </summary>
public class ReindeerManager : IReindeerManager
{
    private int _count = 9;

    public int Count => _count;

    public void UseReindeer()
    {
        _count--;
        if (_count < 6)
        {
            Console.WriteLine("⚠️ Troppe poche renne! Richiamiamo quelle di riserva!");
            RestoreReindeer();
        }
    }

    public void RestoreReindeer()
    {
        _count = 9;
    }
}
