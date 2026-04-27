// KISS — Tecnica: Early Return / Guard Clauses
// Elimina la nidificazione profonda portando i casi-limite in cima al metodo.

// ❌ PRIMA: logica annegata in 4 livelli di if
public Result ValidateBad(Order order)
{
    if (order != null)
    {
        if (order.Items.Any())
        {
            if (order.Customer != null)
            {
                if (order.Customer.IsActive)
                {
                    // la vera logica, dopo quattro livelli di nidificazione
                    return Result.Ok();
                }
                else return Result.Error("Customer inactive");
            }
            else return Result.Error("Customer missing");
        }
        else return Result.Error("Order empty");
    }
    else return Result.Error("Order null");
}

// ✅ DOPO: guard clauses — le pre-condizioni spariscono subito
public Result ValidateGood(Order order)
{
    if (order == null)             return Result.Error("Order null");
    if (!order.Items.Any())        return Result.Error("Order empty");
    if (order.Customer == null)    return Result.Error("Customer missing");
    if (!order.Customer.IsActive)  return Result.Error("Customer inactive");

    // la vera logica, al primo livello
    return Result.Ok();
}

// ──────────────────────────────────────────────────────────────────────────────
// KISS — Tecnica: estrazione di metodi con nomi che spiegano il PERCHÉ

// ❌ PRIMA: espressione booleana da decifrare
if (order.Items.Sum(i => i.Quantity) > 100 &&
    order.Customer.LoyaltyYears >= 5 &&
    !order.HasAppliedDiscount)
{
    order.ApplyDiscount(0.10m);
}

// ✅ DOPO: il chiamante legge una frase in linguaggio di dominio
if (IsEligibleForLoyaltyDiscount(order))
    order.ApplyDiscount(0.10m);

private bool IsEligibleForLoyaltyDiscount(Order order) =>
    order.Items.Sum(i => i.Quantity) > 100 &&
    order.Customer.LoyaltyYears >= 5 &&
    !order.HasAppliedDiscount;

// ──────────────────────────────────────────────────────────────────────────────
// KISS — Tecnica: dispatch tramite dizionario invece di switch lungo

// ❌ PRIMA: switch che cresce ad ogni nuovo formato
public string FormatReportBad(Report report, string format)
{
    switch (format)
    {
        case "json": return new JsonFormatter().Format(report);
        case "xml":  return new XmlFormatter().Format(report);
        case "csv":  return new CsvFormatter().Format(report);
        case "pdf":  return new PdfFormatter().Format(report);
        default: throw new NotSupportedException(format);
    }
}

// ✅ DOPO: aggiungere un formato = aggiungere una entry alla mappa
private static readonly IReadOnlyDictionary<string, IReportFormatter> Formatters =
    new Dictionary<string, IReportFormatter>
    {
        ["json"] = new JsonFormatter(),
        ["xml"]  = new XmlFormatter(),
        ["csv"]  = new CsvFormatter(),
        ["pdf"]  = new PdfFormatter(),
    };

public string FormatReportGood(Report report, string format)
{
    if (!Formatters.TryGetValue(format, out var formatter))
        throw new NotSupportedException(format);
    return formatter.Format(report);
}
