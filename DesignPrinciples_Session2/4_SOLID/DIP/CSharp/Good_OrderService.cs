// DIP — CORRETTO: la business logic dipende da astrazioni (interfacce).
// Le tecnologie concrete sono "plug-in" che il caller inietta nel costruttore.
// OrderService è testabile con fake in memoria, senza DB/SMTP/Stripe reali.

// Astrazioni di proprietà del livello business (non del livello infrastruttura)
public interface IOrderRepository
{
    void Save(Order order);
    Order? FindById(int id);
}

public interface IEmailSender
{
    void Send(string to, string subject, string body = "");
}

public interface IPaymentGateway
{
    PaymentResult Charge(decimal amount, PaymentMethod method);
}

// La business logic dipende SOLO dalle astrazioni
public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IEmailSender _emailSender;
    private readonly IPaymentGateway _payments;

    public OrderService(
        IOrderRepository repository,
        IEmailSender emailSender,
        IPaymentGateway payments)
    {
        _repository = repository;
        _emailSender = emailSender;
        _payments = payments;
    }

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

// Implementazioni concrete — nel progetto Infrastructure, non nel dominio
public class SqlOrderRepository : IOrderRepository
{
    public void Save(Order order) { /* SQL Server */ }
    public Order? FindById(int id) => null; /* ... */
}

public class SmtpEmailSender : IEmailSender
{
    public void Send(string to, string subject, string body = "") { /* SMTP */ }
}

public class StripePaymentGateway : IPaymentGateway
{
    public PaymentResult Charge(decimal amount, PaymentMethod method)
        => new PaymentResult(true, null);
}
