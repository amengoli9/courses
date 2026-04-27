// YAGNI — CORRETTO: una classe, un metodo, esattamente quello che serve oggi.
// Quando arriverà PayPal, avremo due esempi reali per progettare l'interfaccia giusta.

public class StripePaymentService
{
    private readonly StripeClient _stripe;

    public StripePaymentService(StripeClient stripe)
    {
        _stripe = stripe;
    }

    public PaymentResult Charge(decimal amount, string currency, string customerToken)
    {
        var charge = _stripe.Charges.Create(new ChargeCreateOptions
        {
            Amount = (long)(amount * 100),
            Currency = currency,
            Source = customerToken,
        });

        return new PaymentResult(charge.Id, charge.Status);
    }
}
