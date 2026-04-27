// DIP — VIOLAZIONE: la logica di business dipende direttamente da dettagli tecnologici.
// Per cambiare DB, SMTP o Stripe → toccare OrderService.
// Per testare OrderService → serve un DB reale, un server SMTP, un account Stripe.

public class OrderService
{
    // Dipendenze hardcoded: costruite internamente, non iniettabili
    private readonly SqlOrderRepository _repository = new SqlOrderRepository();
    private readonly SmtpEmailSender _emailSender = new SmtpEmailSender();
    private readonly StripePaymentGateway _payments = new StripePaymentGateway();

    public void Place(Order order)
    {
        if (!order.Items.Any())
            throw new InvalidOperationException("Ordine vuoto");

        var paymentResult = _payments.Charge(order.Total, order.Customer.Card);
        if (!paymentResult.Success)
            throw new PaymentFailedException(paymentResult.Error);

        _repository.Save(order);
        _emailSender.Send(order.Customer.Email, "Conferma ordine");
    }
}

// Classi concrete (dettagli tecnologici)
public class SqlOrderRepository
{
    public void Save(Order order) { /* SQL Server */ }
}

public class SmtpEmailSender
{
    public void Send(string to, string subject) { /* SMTP */ }
}

public class StripePaymentGateway
{
    public PaymentResult Charge(decimal amount, object card) => new PaymentResult(true, null);
}
