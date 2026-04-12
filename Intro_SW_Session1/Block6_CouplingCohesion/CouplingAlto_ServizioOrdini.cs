// ===================================================================
// BLOCCO 6 — Coupling e Cohesion
// Sezione 6.1 — Coupling e Cohesion
//
// ❌ COUPLING ALTO — ServizioOrdini conosce i dettagli interni
//    di ogni altra classe. Cambi uno, cambi tutti.
//
// PROBLEMA: Se domani cambiamo database (da SQL Server a PostgreSQL),
// gateway (da Stripe a PayPal), o email provider (da SMTP a SendGrid),
// dobbiamo modificare QUESTA classe. Ma questa classe non dovrebbe
// sapere niente di come funzionano database, pagamenti o email.
// ===================================================================

using System.Net.Mail;
using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block6_CouplingCohesion;

// Classi concrete da cui ServizioOrdiniAltoCoupling dipende direttamente
public class SqlServerDatabase
{
    public void ApriConnessione(string connectionString) { /* ... */ }
    public void EseguiQuery(string sql) { /* ... */ }
    public void ChiudiConnessione() { /* ... */ }
}

public class SmtpEmailSender
{
    public void Configura(string host, int porta, string user, string pass) { /* ... */ }
    public void Invia(string da, string a, string oggetto, string corpo) { /* ... */ }
}

public class StripePaymentGateway
{
    public object CreateCharge(decimal importo, string valuta, string customerId)
    { /* ... */ return null; }
}

// La God Class che dipende da tutto
public class ServizioOrdiniAltoCoupling
{
    // Dipende direttamente dalle classi concrete
    private readonly SqlServerDatabase _database = new SqlServerDatabase();
    private readonly SmtpEmailSender _emailSender = new SmtpEmailSender();
    private readonly StripePaymentGateway _pagamento = new StripePaymentGateway();

    public void CompletaOrdine(Ordine ordine)
    {
        // Conosce i dettagli del database
        _database.ApriConnessione("Server=produzione;Database=ordini;...");
        _database.EseguiQuery(
            $"INSERT INTO Ordini VALUES ('{ordine.Id}', '{ordine.Totale}')");
        _database.ChiudiConnessione();

        // Conosce i dettagli del gateway di pagamento
        var charge = _pagamento.CreateCharge(
            ordine.Totale, "eur", ordine.Cliente.StripeCustomerId);

        // Conosce i dettagli dell'email
        _emailSender.Configura("smtp.azienda.com", 587, "user", "pass");
        _emailSender.Invia(
            "ordini@azienda.com",
            ordine.Cliente.Email,
            "Conferma Ordine",
            $"Il tuo ordine {ordine.Id} è stato confermato.");
    }
}
