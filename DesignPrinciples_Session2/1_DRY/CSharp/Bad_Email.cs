// DRY — VIOLAZIONE: il setup SMTP (server, mittente) è duplicato.
// La "decisione di infrastruttura" (come mandiamo email) vive in due posti.

public class EmailSender
{
    public void SendOrderConfirmation(Order order)
    {
        var smtp = new SmtpClient("smtp.azienda.it");
        var message = new MailMessage();
        message.From = new MailAddress("noreply@azienda.it");
        message.To.Add(order.CustomerEmail);
        message.Subject = $"Conferma ordine #{order.Id}";
        message.Body = $"Gentile cliente, confermiamo l'ordine #{order.Id} per €{order.Total:F2}.";
        smtp.Send(message);
    }

    public void SendShippingNotification(Order order)
    {
        var smtp = new SmtpClient("smtp.azienda.it");
        var message = new MailMessage();
        message.From = new MailAddress("noreply@azienda.it");
        message.To.Add(order.CustomerEmail);
        message.Subject = $"Spedizione ordine #{order.Id}";
        message.Body = $"Gentile cliente, l'ordine #{order.Id} è stato spedito.";
        smtp.Send(message);
    }
}
