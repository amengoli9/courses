// ===================================================================
// BLOCCO 6 — Coupling e Cohesion
// Sezione 6.1 — Coupling e Cohesion
//
// ✅ COUPLING BASSO — Dipende da interfacce, non da implementazioni.
//
// ServizioOrdini non sa se il database è SQL Server o MongoDB.
// Non sa se i pagamenti passano per Stripe o PayPal.
// Non sa se le email partono via SMTP o via SendGrid.
// Sa solo che esiste qualcosa che salva, processa, notifica.
//
// VANTAGGIO: Se domani cambiamo database, creiamo un nuovo
// PostgresOrdineRepository che implementa IOrdineRepository.
// ServizioOrdini non cambia. Non sa nemmeno che è successo qualcosa.
//
// Questo è il principio su cui si basa la D di SOLID:
// Dependency Inversion — dipendere da interfacce (astrazioni),
// non da classi concrete.
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block6_CouplingCohesion;

// --- Interfacce: astrazioni che nascondono i dettagli ---

public interface IOrdineRepository
{
    void Salva(Ordine ordine);
}

public interface IServizioNotifiche
{
    void NotificaConfermaOrdine(Ordine ordine);
}

public interface IServizioPagamenti
{
    PagamentoResult ProcessaPagamento(Ordine ordine);
}

// --- Implementazioni concrete (potrebbero cambiare liberamente) ---

public class SqlServerOrdineRepository : IOrdineRepository
{
    public void Salva(Ordine ordine) { /* salva su SQL Server */ }
}

public class SendGridNotifiche : IServizioNotifiche
{
    public void NotificaConfermaOrdine(Ordine ordine) { /* manda via SendGrid */ }
}

public class StripeServizioPagamenti : IServizioPagamenti
{
    public PagamentoResult ProcessaPagamento(Ordine ordine)
    { /* processa con Stripe */ return new PagamentoResult(); }
}

// --- ServizioOrdini: dipende solo dalle interfacce ---

public class ServizioOrdiniBassoCoupling
{
    private readonly IOrdineRepository _repository;
    private readonly IServizioPagamenti _pagamenti;
    private readonly IServizioNotifiche _notifiche;

    public ServizioOrdiniBassoCoupling(
        IOrdineRepository repository,
        IServizioPagamenti pagamenti,
        IServizioNotifiche notifiche)
    {
        _repository = repository;
        _pagamenti = pagamenti;
        _notifiche = notifiche;
    }

    public void CompletaOrdine(Ordine ordine)
    {
        _repository.Salva(ordine);
        _pagamenti.ProcessaPagamento(ordine);
        _notifiche.NotificaConfermaOrdine(ordine);
    }
}
