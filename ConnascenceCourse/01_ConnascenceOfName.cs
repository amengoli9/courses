/*
 * ============================================================================
 * 1. CONNASCENCE OF NAME (CoN)
 * ============================================================================
 *
 * DEFINIZIONE: Più componenti devono concordare sullo stesso NOME
 * FORZA: ⭐ DEBOLE (accettabile)
 *
 * ============================================================================
 */

// ============================================================================
// ESEMPIO SEMPLICE PER SLIDE
// ============================================================================

/*
PROBLEMA:
    emailService.SendEmail()  // Dipende dal nome "SendEmail"

SOLUZIONE:
    INotificationService.Send()  // Interfaccia più stabile
*/

// ============================================================================
// ESEMPIO C#
// ============================================================================

namespace ConnascenceCourse.ConnascenceOfName
{
    // ❌ PROBLEMA
    public class EmailService
    {
        public void SendEmail(string to, string message)
        {
            Console.WriteLine($"Email to {to}");
        }
    }

    public class OrderProcessor
    {
        private EmailService emailService; // Accoppiato al nome concreto

        public void ProcessOrder()
        {
            emailService.SendEmail("customer@example.com", "Order confirmed");
        }
    }

    // ✅ SOLUZIONE
    public interface INotificationService
    {
        void Send(string to, string message);
    }

    public class EmailNotificationService : INotificationService
    {
        public void Send(string to, string message)
        {
            Console.WriteLine($"Email to {to}");
        }
    }

    public class OrderProcessorRefactored
    {
        private readonly INotificationService _notificationService; // Dipende dall'interfaccia

        public OrderProcessorRefactored(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void ProcessOrder()
        {
            _notificationService.Send("customer@example.com", "Order confirmed");
        }
    }

    // VANTAGGI: Posso rinominare EmailNotificationService senza impattare OrderProcessor
}
