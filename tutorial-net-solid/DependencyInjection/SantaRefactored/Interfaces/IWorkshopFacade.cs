namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Facade per l'intero workshop
/// </summary>
public interface IWorkshopFacade
{
    void ProcessChristmasLetter(string childName, int age, string behavior,
        string toyType, string country, bool isChristmasEve);
    void DeliverPresent(string deliveryType, int toyIndex);
    void GenerateChristmasReport();
    void SaveToNorthPoleDatabase();
    void SendToSantasPrivateFax();
}
