using SantasWorkshop.Interfaces;

namespace SantasWorkshop.Services;

/// <summary>
/// Implementazione del gestore energia elfi
/// </summary>
public class ElfEnergyManager : IElfEnergyManager
{
    private int _energy = 100;

    public int CurrentEnergy => _energy;

    public void ConsumeEnergy(int amount)
    {
        _energy -= amount;
    }

    public bool NeedsRecharge() => _energy < 20;

    public void Recharge()
    {
        Console.WriteLine("⚡ ATTENZIONE: Energia elfi bassa! Pausa biscotti necessaria!");
        _energy = 100;
        Console.WriteLine("🍪 Elfi hanno mangiato biscotti! Energia ripristinata!");
    }
}
