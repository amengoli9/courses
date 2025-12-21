using SantasWorkshop.Interfaces;

namespace SantasWorkshop.Infrastructure;

/// <summary>
/// Implementazione del servizio fax (console)
/// </summary>
public class ConsoleFaxService : IFaxService
{
    public void SendFax()
    {
        Console.WriteLine("\n📠 INVIO FAX A BABBO NATALE...");
        Console.WriteLine("☎️  Composizione numero: +999-NORTHPOLE");
        Console.WriteLine("📄 *beep* *boop* *screech*");
        Console.WriteLine("✅ Fax inviato con successo!");
    }
}
