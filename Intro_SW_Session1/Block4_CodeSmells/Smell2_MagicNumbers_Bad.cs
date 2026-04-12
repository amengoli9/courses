// ===================================================================
// BLOCCO 4 — Code Smells: Dare un Nome ai Problemi
// Smell 2: Magic Numbers e Magic Strings
//
// ❌ Magic numbers e magic strings ovunque.
// '0.22' — cosa significa? 'Gold' — e se qualcuno scrive 'gold'?
// ===================================================================

namespace Intro_SW_Session1.Block4_CodeSmells;

public class CalcoloPreventivoBad
{
    public decimal CalcolaPreventivo(decimal importoBase, string tipo, int quantita)
    {
        decimal risultato;

        if (tipo == "standard")                     // magic string
            risultato = importoBase * 1.0m;
        else if (tipo == "premium")                 // magic string
            risultato = importoBase * 1.35m;        // 1.35? perché?
        else if (tipo == "enterprise")              // magic string
            risultato = importoBase * 1.65m;        // 1.65? perché?
        else
            risultato = importoBase * 2.0m;         // 2.0? chi l'ha deciso?

        if (quantita > 100)                         // perché 100?
            risultato *= 0.90m;                     // 10% sconto? dove è documentato?
        else if (quantita > 50)                     // perché 50?
            risultato *= 0.95m;                     // 5%? e il 100 sopra?

        risultato += 25.00m;                        // 25€ di cosa?

        if (risultato > 10000m)                     // perché 10000?
            risultato -= 500m;                      // 500€ di sconto? perché?

        return risultato;
    }
}
