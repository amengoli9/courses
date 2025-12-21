using SantasWorkshop.Interfaces;

namespace SantasWorkshop.Services;

/// <summary>
/// Implementazione del servizio di assegnazione elfi
/// [O] Per aggiungere nuovi paesi, si può creare un decorator o una nuova implementazione
/// </summary>
public class ElfAssignmentService : IElfAssignmentService
{
    private readonly Dictionary<string, string> _elfAssignments = new()
    {
        ["Italia"] = "Pasqualino",
        ["USA"] = "Jingles",
        ["Giappone"] = "Yuki"
    };

    public string AssignElf(string country)
    {
        if (_elfAssignments.TryGetValue(country, out var elf))
        {
            Console.WriteLine($"🧝 Elfo {GetElfNationality(country)} {elf} assegnato!");
            return elf;
        }

        Console.WriteLine("🧝 Elfo generico Buddy assegnato!");
        return "Buddy";
    }

    private string GetElfNationality(string country) => country switch
    {
        "Italia" => "italiano",
        "USA" => "americano",
        "Giappone" => "giapponese",
        _ => "generico"
    };
}
