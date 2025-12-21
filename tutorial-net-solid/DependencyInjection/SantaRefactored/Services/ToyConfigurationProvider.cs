using SantasWorkshop.Interfaces;
using SantasWorkshop.Models;

namespace SantasWorkshop.Services;

/// <summary>
/// Implementazione del provider di configurazione giocattoli
/// [O] Per aggiungere nuovi tipi, si può creare un decorator o una nuova implementazione
/// </summary>
public class ToyConfigurationProvider : IToyConfigurationProvider
{
    private readonly Dictionary<string, ToyProductionConfig> _configurations = new()
    {
        ["Trenino"] = new ToyProductionConfig { ProductionTime = 5, MaterialsNeeded = 10, Description = "🚂 Trenino: complessità media" },
        ["Bambola"] = new ToyProductionConfig { ProductionTime = 3, MaterialsNeeded = 7, Description = "👧 Bambola: complessità bassa" },
        ["VideoGame"] = new ToyProductionConfig { ProductionTime = 8, MaterialsNeeded = 15, Description = "🎮 VideoGioco: complessità alta" },
        ["Puzzle"] = new ToyProductionConfig { ProductionTime = 2, MaterialsNeeded = 5, Description = "🧩 Puzzle: complessità molto bassa" },
        ["Bicicletta"] = new ToyProductionConfig { ProductionTime = 10, MaterialsNeeded = 20, Description = "🚲 Bicicletta: complessità molto alta" }
    };

    public ToyProductionConfig GetConfiguration(string toyType)
    {
        if (_configurations.TryGetValue(toyType, out var config))
        {
            return config;
        }

        return new ToyProductionConfig
        {
            ProductionTime = 4,
            MaterialsNeeded = 8,
            Description = "🎁 Giocattolo generico"
        };
    }
}
