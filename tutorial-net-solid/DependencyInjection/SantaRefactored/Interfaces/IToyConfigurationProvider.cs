using SantasWorkshop.Models;

namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Fornisce configurazione per i tipi di giocattolo
/// [O] Estendibile tramite nuove implementazioni
/// </summary>
public interface IToyConfigurationProvider
{
    ToyProductionConfig GetConfiguration(string toyType);
}
