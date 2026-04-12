// ===================================================================
// BLOCCO 1 — Qualità del Progetto: La Visione dall'Alto
// Sezione 1.2 — Observability: Sapere cosa succede in produzione
//
// Esempio: Logging fatto bene vs fatto male
// ===================================================================

using Microsoft.Extensions.Logging;
using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block1_QualitaProgetto;

// ===================================================================
// ❌ MALE — Log inutile, non dice niente
// ===================================================================
public class CalcolatoreScontoBadLogging
{
    public decimal CalcolaSconto(Ordine ordine)
    {
        Console.WriteLine("Calcolo sconto");  // Cosa?? Quale ordine? Che sconto?

        try
        {
            var sconto = ordine.Totale * 0.1m;
            Console.WriteLine("Fatto");  // Fatto cosa? Quanto è lo sconto?
            return sconto;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Errore");  // Quale errore? Che informazioni ho?
            throw;
        }
    }
}

// ===================================================================
// ✅ BENE — Log strutturato con contesto
// ===================================================================
public class CalcolatoreScontoGoodLogging
{
    private readonly ILogger<CalcolatoreScontoGoodLogging> _logger;

    public CalcolatoreScontoGoodLogging(ILogger<CalcolatoreScontoGoodLogging> logger)
    {
        _logger = logger;
    }

    public decimal CalcolaSconto(Ordine ordine)
    {
        _logger.LogInformation(
            "Inizio calcolo sconto per ordine {OrdineId}, cliente {ClienteId}, totale {Totale}",
            ordine.Id, ordine.ClienteId, ordine.Totale);

        try
        {
            var sconto = ordine.Totale * 0.1m;

            _logger.LogInformation(
                "Sconto calcolato per ordine {OrdineId}: {Sconto} ({Percentuale}%)",
                ordine.Id, sconto, 10);

            return sconto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Errore nel calcolo sconto per ordine {OrdineId}, cliente {ClienteId}",
                ordine.Id, ordine.ClienteId);
            throw;
        }
    }
}
