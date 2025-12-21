using SantasWorkshop.Interfaces;
using SantasWorkshop.Models;

namespace SantasWorkshop.Services;

/// <summary>
/// Facade che espone le stesse operazioni del WorkshopManager originale
/// ma delegando a servizi specializzati
/// </summary>
public class WorkshopFacade(
        ILetterProcessor letterProcessor,
        IDeliveryService deliveryService,
        IReportGenerator reportGenerator,
        IDatabaseService databaseService,
        IFaxService faxService,
        IChildRepository childRepository,
        IToyRepository toyRepository,
        IElfEnergyManager elfEnergyManager,
        IReindeerManager reindeerManager) : IWorkshopFacade
{

    public void ProcessChristmasLetter(string childName, int age, string behavior,
        string toyType, string country, bool isChristmasEve)
    {
        letterProcessor.ProcessLetter(new LetterRequest
        {
            ChildName = childName,
            Age = age,
            Behavior = behavior,
            ToyType = toyType,
            Country = country,
            IsChristmasEve = isChristmasEve
        });
    }

    public void DeliverPresent(string deliveryType, int toyIndex)
    {
        deliveryService.DeliverPresent(deliveryType, toyIndex);
    }

    public void GenerateChristmasReport()
    {
        reportGenerator.GenerateReport(
            childRepository.GetAll(),
            toyRepository.GetAll(),
            elfEnergyManager.CurrentEnergy,
            reindeerManager.Count);
    }

    public void SaveToNorthPoleDatabase()
    {
        databaseService.SaveAll(childRepository.GetAll(), toyRepository.GetAll());
    }

    public void SendToSantasPrivateFax()
    {
        faxService.SendFax();
    }
}
