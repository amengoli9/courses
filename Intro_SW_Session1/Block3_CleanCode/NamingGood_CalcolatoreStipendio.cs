// ===================================================================
// BLOCCO 3 — Clean Code: I Principi di Robert C. Martin
// Sezione 3.1 — Naming: Il principio più importante
//
// ✅ VERSIONE BUONA — Ora capisco cosa calcoliamo
// e cosa significano i parametri.
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block3_CleanCode;

public class CalcolatoreStipendio
{
    public decimal CalcolaRetribuzioneLorda(
        decimal stipendioBase,
        decimal bonus,
        TipoCalcolo tipoCalcolo)
    {
        return tipoCalcolo switch
        {
            TipoCalcolo.Addizione => stipendioBase + bonus,
            TipoCalcolo.Sottrazione => stipendioBase - bonus,
            TipoCalcolo.Moltiplicazione => stipendioBase * bonus,
            _ => throw new ArgumentOutOfRangeException(nameof(tipoCalcolo))
        };
    }
}
