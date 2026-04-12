// ===================================================================
// BLOCCO 2 — Qualità del Codice e Debito Tecnico
// Sezione 2.1 — Cosa rende il codice "di qualità"
//
// VERSIONE B — La stessa logica, qualità alta
// I nomi dicono cosa fa il codice. Ogni metodo fa una cosa sola.
// I casi limite sono gestiti.
// ===================================================================

namespace Intro_SW_Session1.Block2_QualitaCodiceDebitoTecnico;

public class CalcolatoreStatistiche
{
    public double CalcolaSommaPositivi(IReadOnlyList<double> valori)
    {
        return valori.Where(v => v > 0).Sum();
    }

    public double CalcolaMedia(IReadOnlyList<double> valori)
    {
        if (valori.Count == 0)
            throw new InvalidOperationException(
                "Impossibile calcolare la media di una lista vuota.");

        return valori.Average();
    }

    public double CalcolaSommaNegativi(IReadOnlyList<double> valori)
    {
        return valori.Where(v => v < 0).Sum();
    }
}
