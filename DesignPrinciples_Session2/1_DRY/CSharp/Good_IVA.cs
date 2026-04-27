// DRY — CORRETTO: la conoscenza "aliquota IVA italiana" vive in un solo punto.
// Un cambio normativo = una sola modifica, zero rischi di disallineamento.

public static class TaxRules
{
    // Aliquota IVA ordinaria italiana.
    // Riferimento normativo: DPR 633/1972, aggiornato al 2024.
    public const decimal StandardVatRate = 0.22m;

    public static decimal ApplyVat(decimal net) => net * (1 + StandardVatRate);
}

public class OrderService
{
    public decimal CalculateGrossTotal(Order order)
    {
        var net = order.Items.Sum(i => i.Price * i.Quantity);
        return TaxRules.ApplyVat(net);
    }
}

public class InvoicePrinter
{
    public string PrintTotal(Invoice invoice)
    {
        var net = invoice.Lines.Sum(l => l.Amount);
        return $"Totale: {TaxRules.ApplyVat(net):C}";
    }
}

public class QuoteGenerator
{
    public Quote BuildQuote(IEnumerable<Item> items)
    {
        var net = items.Sum(i => i.Price);
        return new Quote
        {
            Net = net,
            Gross = TaxRules.ApplyVat(net)
        };
    }
}
