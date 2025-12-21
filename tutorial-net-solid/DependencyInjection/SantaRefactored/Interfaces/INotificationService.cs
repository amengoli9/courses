using SantasWorkshop.Models;

namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Servizio di notifica
/// [D] Astrazione per le notifiche
/// </summary>
public interface INotificationService
{
    void SendProductionNotification(Child child, Toy toy, string assignedElf);
}
