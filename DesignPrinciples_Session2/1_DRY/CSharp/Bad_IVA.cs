// DRY — VIOLAZIONE: l'aliquota IVA 22% è duplicata tre volte.
// Se cambia (come già successo), vanno aggiornati tutti i punti.

public class OrderService
{
    public decimal CalculateGrossTotal(Order order)
    {
        var net = order.Items.Sum(i => i.Price * i.Quantity);
        return net * 1.22m;   // IVA italiana
    }
}

public class InvoicePrinter
{
    public string PrintTotal(Invoice invoice)
    {
        var net = invoice.Lines.Sum(l => l.Amount);
        var gross = net * 1.22m;   // IVA italiana, di nuovo
        return $"Totale: {gross:C}";
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
            Gross = net * 1.22m   // IVA italiana, ancora
        };
    }
}
