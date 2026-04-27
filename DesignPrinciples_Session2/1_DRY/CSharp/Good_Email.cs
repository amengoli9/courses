// DRY — CORRETTO: il setup infrastrutturale è centralizzato in Send().
// Subject e body restano nei chiamanti: sono "cosa dire", non "come inviare".

public class EmailSender
{
    private const string SmtpServer = "smtp.azienda.it";
    private const string FromAddress = "noreply@azienda.it";

    public void SendOrderConfirmation(Order order)
    {
        Send(
            to: order.CustomerEmail,
            subject: $"Conferma ordine #{order.Id}",
            body: $"Gentile cliente, confermiamo l'ordine #{order.Id} per €{order.Total:F2}."
        );
    }

    public void SendShippingNotification(Order order)
    {
        Send(
            to: order.CustomerEmail,
            subject: $"Spedizione ordine #{order.Id}",
            body: $"Gentile cliente, l'ordine #{order.Id} è stato spedito."
        );
    }

    private void Send(string to, string subject, string body)
    {
        var smtp = new SmtpClient(SmtpServer);
        var message = new MailMessage
        {
            From = new MailAddress(FromAddress),
            Subject = subject,
            Body = body
        };
        message.To.Add(to);
        smtp.Send(message);
    }
}
