// ===================================================================
// BLOCCO 4 — Code Smells: Dare un Nome ai Problemi
// Smell 5: Feature Envy
//
// ❌ FEATURE ENVY — Il metodo CalcolaStipendioNetto "invidia" i dati
// di Dipendente. Usa 8 proprietà di Dipendente e 0 proprietà proprie.
// Se qualcuno cambia la struttura di Dipendente, deve cambiare anche
// CalcoloBustaPaga. Questo è accoppiamento stretto (coupling alto).
// ===================================================================

namespace Intro_SW_Session1.Block4_CodeSmells;

public class DipendenteBad
{
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public decimal StipendioBase { get; set; }
    public int AnniDiServizio { get; set; }
    public string Livello { get; set; }       // "Junior", "Mid", "Senior"
    public bool IsPartTime { get; set; }
    public decimal PercentualePartTime { get; set; }  // es. 0.5 per 50%
    public string Citta { get; set; }
}

public class CalcoloBustaPagaBad
{
    // Questo metodo usa 8 proprietà di Dipendente e 0 proprietà proprie.
    // Chiaramente "invidia" i dati di Dipendente.
    public decimal CalcolaStipendioNetto(DipendenteBad d)
    {
        var lordo = d.StipendioBase;

        // Bonus anzianità
        if (d.AnniDiServizio > 10)
            lordo += d.StipendioBase * 0.10m;
        else if (d.AnniDiServizio > 5)
            lordo += d.StipendioBase * 0.05m;

        // Bonus livello
        if (d.Livello == "Senior")
            lordo += 500m;
        else if (d.Livello == "Mid")
            lordo += 250m;

        // Part-time
        if (d.IsPartTime)
            lordo *= d.PercentualePartTime;

        // Bonus città
        if (d.Citta == "Milano" || d.Citta == "Roma")
            lordo += 200m;

        // Tasse (semplificato)
        var netto = lordo * 0.70m;

        return netto;
    }
}
