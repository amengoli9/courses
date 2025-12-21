namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Gestisce l'energia degli elfi
/// </summary>
public interface IElfEnergyManager
{
    int CurrentEnergy { get; }
    void ConsumeEnergy(int amount);
    bool NeedsRecharge();
    void Recharge();
}
