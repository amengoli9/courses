// YAGNI — VIOLAZIONE: architettura da "multi-provider" quando esiste solo Stripe.
// Registry, Router, Strategy, interfacce astratte: tutto inutilizzato oggi.
// Ogni nuova astrazione costa: costruzione, manutenzione, overhead cognitivo.

public interface IPaymentProvider
{
    PaymentResult Charge(decimal amount, string currency, PaymentMethod method);
    PaymentResult Refund(string transactionId, decimal? amount = null);
    Task<PaymentResult> ChargeAsync(decimal amount, string currency, PaymentMethod method);
    Task<PaymentResult> RefundAsync(string transactionId, decimal? amount = null);
    PaymentProviderCapabilities Capabilities { get; }
}

public class PaymentProviderRegistry
{
    private readonly Dictionary<string, IPaymentProvider> _providers = new();

    public void Register(string name, IPaymentProvider provider) =>
        _providers[name] = provider;

    public IPaymentProvider Get(string name) =>
        _providers.TryGetValue(name, out var p)
            ? p
            : throw new InvalidOperationException($"Provider {name} non registrato");
}

public class PaymentRouter
{
    private readonly PaymentProviderRegistry _registry;
    private readonly IPaymentRoutingStrategy _strategy;

    public PaymentRouter(PaymentProviderRegistry registry, IPaymentRoutingStrategy strategy)
    {
        _registry = registry;
        _strategy = strategy;
    }

    public PaymentResult Charge(Payment payment)
    {
        var providerName = _strategy.SelectProvider(payment);
        var provider = _registry.Get(providerName);
        return provider.Charge(payment.Amount, payment.Currency, payment.Method);
    }
}

public interface IPaymentRoutingStrategy
{
    string SelectProvider(Payment payment);
}

// L'unico implementatore: Stripe. Sempre. Nessun altro all'orizzonte.
public class StripeProvider : IPaymentProvider
{
    public PaymentProviderCapabilities Capabilities { get; } = new();

    public PaymentResult Charge(decimal amount, string currency, PaymentMethod method)
        => CallStripeApi(amount, currency, method);

    public PaymentResult Refund(string transactionId, decimal? amount = null)
        => CallStripeRefundApi(transactionId, amount);

    public Task<PaymentResult> ChargeAsync(decimal amount, string currency, PaymentMethod method)
        => Task.FromResult(Charge(amount, currency, method));

    public Task<PaymentResult> RefundAsync(string transactionId, decimal? amount = null)
        => Task.FromResult(Refund(transactionId, amount));

    private PaymentResult CallStripeApi(decimal amount, string currency, PaymentMethod method)
        => throw new NotImplementedException();

    private PaymentResult CallStripeRefundApi(string transactionId, decimal? amount)
        => throw new NotImplementedException();
}
