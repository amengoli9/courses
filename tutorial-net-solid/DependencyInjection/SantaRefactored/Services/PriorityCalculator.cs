using SantasWorkshop.Interfaces;

namespace SantasWorkshop.Services;

/// <summary>
/// Implementazione del calcolatore di priorità
/// </summary>
public class PriorityCalculator : IPriorityCalculator
{
    public int CalculatePriority(bool isChristmasEve, int age, string country)
    {
        if (isChristmasEve)
        {
            Console.WriteLine("🔥 URGENTE: Vigilia di Natale! Priorità massima!");
            return 1;
        }
        else if (age < 5)
        {
            Console.WriteLine("👶 Bambino piccolo: priorità alta");
            return 2;
        }
        else if (country == "Italia" || country == "Polo Nord")
        {
            Console.WriteLine("🌍 Paese vicino: priorità media");
            return 3;
        }
        else
        {
            Console.WriteLine("✈️ Paese lontano: priorità normale");
            return 4;
        }
    }
}
