// ===================================================================
// BLOCCO 2 — Qualità del Codice e Debito Tecnico
// Sezione 2.2 — Il Debito Tecnico
//
// Esempio: Debito tecnico deliberato e prudente
// CONTESTO: Dobbiamo lanciare entro venerdì.
// Il calcolo dello sconto per ora è hardcodato.
// DECISIONE: lo sappiamo, lo segniamo, ci torniamo.
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block2_QualitaCodiceDebitoTecnico;

public class CalcolatoreSconto
{
    // TODO: DEBITO TECNICO — Ticket PROJ-1234
    // Lo sconto è hardcodato al 10%. Il requisito finale prevede
    // regole di sconto per categoria cliente (Bronze/Silver/Gold),
    // per volume ordine, e per campagne promozionali.
    // Stimiamo 3 giorni di lavoro per implementare la versione completa.
    // Data decisione: 2025-03-15, approvato da: Product Owner
    // Pianificato per: Sprint 12
    public decimal CalcolaSconto(Ordine ordine)
    {
        const decimal percentualeSconto = 0.10m;
        return ordine.Totale * percentualeSconto;
    }
}
