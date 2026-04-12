// ===================================================================
// BLOCCO 4 — Code Smells: Dare un Nome ai Problemi
// Smell 2: Magic Numbers e Magic Strings
//
// ✅ Costanti con nomi significativi + enum.
// Ogni valore ha un nome. Se cambia lo sconto, sapete dove andare.
// Con l'enum il compilatore vi protegge: non potete sbagliare.
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block4_CodeSmells;

public class CalcoloPreventivoGood
{
    private const decimal MoltiplicatorePremium = 1.35m;
    private const decimal MoltiplicatoreEnterprise = 1.65m;
    private const decimal MoltiplicatoreCustom = 2.00m;

    private const int SogliaQuantitaGrande = 100;
    private const int SogliaQuantitaMedia = 50;
    private const decimal ScontoQuantitaGrande = 0.90m;
    private const decimal ScontoQuantitaMedia = 0.95m;

    private const decimal CostoFissoSetup = 25.00m;

    private const decimal SogliaOrdineGrande = 10_000m;
    private const decimal ScontoOrdineGrande = 500m;

    public decimal CalcolaPreventivo(
        decimal importoBase, TipoPiano piano, int quantita)
    {
        var importoConPiano = ApplicaMoltiplicatorePiano(importoBase, piano);
        var importoConScontoVolume = ApplicaScontoVolume(importoConPiano, quantita);
        var importoConSetup = importoConScontoVolume + CostoFissoSetup;
        var importoFinale = ApplicaScontoOrdineGrande(importoConSetup);

        return importoFinale;
    }

    private decimal ApplicaMoltiplicatorePiano(decimal importo, TipoPiano piano)
    {
        return piano switch
        {
            TipoPiano.Standard => importo,
            TipoPiano.Premium => importo * MoltiplicatorePremium,
            TipoPiano.Enterprise => importo * MoltiplicatoreEnterprise,
            _ => importo * MoltiplicatoreCustom
        };
    }

    private decimal ApplicaScontoVolume(decimal importo, int quantita)
    {
        if (quantita > SogliaQuantitaGrande)
            return importo * ScontoQuantitaGrande;

        if (quantita > SogliaQuantitaMedia)
            return importo * ScontoQuantitaMedia;

        return importo;
    }

    private decimal ApplicaScontoOrdineGrande(decimal importo)
    {
        return importo > SogliaOrdineGrande
            ? importo - ScontoOrdineGrande
            : importo;
    }
}
