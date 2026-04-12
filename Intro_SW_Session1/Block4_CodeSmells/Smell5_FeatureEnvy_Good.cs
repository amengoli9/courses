// ===================================================================
// BLOCCO 4 — Code Smells: Dare un Nome ai Problemi
// Smell 5: Feature Envy
//
// ✅ La logica è dove stanno i dati.
// Il Dipendente sa calcolare il proprio stipendio lordo perché
// ha tutte le informazioni. CalcoloBustaPaga chiede solo il lordo
// e applica le tasse. Se domani il calcolo del bonus cambia,
// toccate solo Dipendente.
// ===================================================================

namespace Intro_SW_Session1.Block4_CodeSmells;

public class Dipendente
{
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public decimal StipendioBase { get; set; }
    public int AnniDiServizio { get; set; }
    public string Livello { get; set; }
    public bool IsPartTime { get; set; }
    public decimal PercentualePartTime { get; set; }
    public string Citta { get; set; }

    // La logica sta dove stanno i dati — il Dipendente sa calcolare
    // il proprio stipendio lordo perché ha tutte le informazioni.
    public decimal CalcolaStipendioLordo()
    {
        var lordo = StipendioBase;
        lordo += CalcolaBonusAnzianita();
        lordo += CalcolaBonusLivello();
        lordo = ApplicaPartTime(lordo);
        lordo += CalcolaBonusCitta();
        return lordo;
    }

    private decimal CalcolaBonusAnzianita()
    {
        if (AnniDiServizio > 10) return StipendioBase * 0.10m;
        if (AnniDiServizio > 5) return StipendioBase * 0.05m;
        return 0m;
    }

    private decimal CalcolaBonusLivello()
    {
        return Livello switch
        {
            "Senior" => 500m,
            "Mid" => 250m,
            _ => 0m
        };
    }

    private decimal ApplicaPartTime(decimal importo)
    {
        return IsPartTime ? importo * PercentualePartTime : importo;
    }

    private decimal CalcolaBonusCitta()
    {
        return (Citta == "Milano" || Citta == "Roma") ? 200m : 0m;
    }
}

public class CalcoloBustaPaga
{
    private const decimal AliquotaFiscale = 0.30m;

    // Ora questo metodo fa solo il suo lavoro: applicare le tasse.
    // Non deve sapere come il dipendente calcola il lordo.
    public decimal CalcolaStipendioNetto(Dipendente dipendente)
    {
        var lordo = dipendente.CalcolaStipendioLordo();
        return lordo * (1 - AliquotaFiscale);
    }
}
