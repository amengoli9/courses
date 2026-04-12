// ===================================================================
// BLOCCO 3 — Clean Code: I Principi di Robert C. Martin
// Sezione 3.3 — Commenti: quando servono e quando sono un code smell
//
// ✅ COMMENTI BUONI — Aggiungono informazione che il codice non può dare:
//    - XML doc per API pubblica
//    - PERCHÉ: spiega il motivo, non il "cosa"
//    - WARNING: protegge da modifiche pericolose
// ===================================================================

namespace Intro_SW_Session1.Block3_CleanCode;

public class CalcolatoreIva
{
    /// <summary>
    /// Calcola l'IVA applicabile all'ordine in base al paese di destinazione.
    /// </summary>
    /// <param name="importo">Importo netto su cui calcolare l'IVA.</param>
    /// <param name="codicePaese">Codice ISO 3166-1 alpha-2 del paese.</param>
    /// <returns>L'importo dell'IVA. Zero se il paese non è in EU.</returns>
    /// <exception cref="ArgumentException">Se il codice paese è nullo o vuoto.</exception>
    public decimal CalcolaIva(decimal importo, string codicePaese)
    {
        // PERCHÉ: L'ordine di lookup è importante. Prima controlliamo le
        // esenzioni speciali (es. zone franche), poi le aliquote ridotte,
        // infine l'aliquota standard. Invertire l'ordine causerebbe
        // la sovrascrittura delle esenzioni con l'aliquota standard.
        // Vedi: https://wiki.azienda.com/fiscal/vat-rules

        if (IsEsente(codicePaese))
            return 0m;

        if (HasAliquotaRidotta(codicePaese))
            return importo * GetAliquotaRidotta(codicePaese);

        return importo * GetAliquotaStandard(codicePaese);
    }

    // WARNING: Non rimuovere il lock. Questo dizionario viene aggiornato
    // dal background job FiscalRatesUpdater ogni 24h. Senza lock,
    // le letture concorrenti durante l'aggiornamento restituiscono
    // valori parziali. Bug #4521.
    private readonly object _lockAliquote = new object();
    private Dictionary<string, decimal> _aliquote = new();

    public decimal GetAliquotaStandard(string codicePaese)
    {
        lock (_lockAliquote)
        {
            return _aliquote.TryGetValue(codicePaese, out var aliquota)
                ? aliquota
                : 0.22m; // Default: aliquota italiana
        }
    }

    private bool IsEsente(string codicePaese)
    {
        // Stub: zone franche, paesi extra-EU, ecc.
        return false;
    }

    private bool HasAliquotaRidotta(string codicePaese)
    {
        // Stub: paesi con aliquota ridotta
        return false;
    }

    private decimal GetAliquotaRidotta(string codicePaese)
    {
        // Stub
        return 0.10m;
    }
}
