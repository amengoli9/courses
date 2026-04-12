// ===================================================================
// BLOCCO 5 — Complessità Ciclomatica e Metriche
// Sezione 5.1 — Cos'è la complessità ciclomatica
//
// Calcoliamo insieme la complessità ciclomatica di questo metodo.
//
// Come si calcola:
//   - Si parte da 1 (il percorso base)
//   - Si aggiunge 1 per ogni: if, else if, case, for, foreach,
//     while, do-while, catch, &&, ||, ??, operatore ternario ?:
//
// COMPLESSITÀ CICLOMATICA FINALE: 14
// Significa 14 percorsi indipendenti nel codice.
// Per testare completamente questo metodo servono ALMENO 14 test case.
//
// Scala di interpretazione:
//   1-5:  semplice, facile da testare
//   6-10: moderata, attenzione
//   11-20: complessa, difficile da manutenere e testare
//   21+:  molto complessa, candidato al refactoring
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block5_ComplessitaCiclomatica;

public class ValidatoreOrdineAltaComplessita
{
    public string ValidaOrdine(Ordine ordine)
    {
        // CC = 1 (percorso base)

        if (ordine == null)                                    // +1 → CC = 2
            return "Ordine nullo";

        if (ordine.Prodotti == null ||                         // +1 → CC = 3
            ordine.Prodotti.Count == 0)                        // +1 → CC = 4
            return "Nessun prodotto nell'ordine";

        if (ordine.Cliente == null)                            // +1 → CC = 5
            return "Cliente non specificato";

        foreach (var prodotto in ordine.Prodotti)              // +1 → CC = 6
        {
            if (prodotto.Quantita <= 0)                        // +1 → CC = 7
                return $"Quantità non valida per {prodotto.Nome}";

            if (prodotto.Prezzo < 0)                           // +1 → CC = 8
                return $"Prezzo negativo per {prodotto.Nome}";

            if (prodotto.Prezzo == 0 &&                        // +1 → CC = 9
                !prodotto.IsOmaggio)                           // +1 → CC = 10
                return $"Prezzo zero per {prodotto.Nome} non marcato come omaggio";
        }

        var totale = ordine.Prodotti.Sum(p => p.Prezzo * p.Quantita);

        if (totale > 50000 &&                                  // +1 → CC = 11
            !ordine.Cliente.IsApprovato)                       // +1 → CC = 12
            return "Ordine sopra i 50.000€ richiede approvazione";

        if (ordine.DataConsegna < DateTime.Today)              // +1 → CC = 13
            return "Data di consegna nel passato";

        if (ordine.DataConsegna > DateTime.Today.AddYears(1))  // +1 → CC = 14
            return "Data di consegna troppo nel futuro";

        return "OK";
    }
    // COMPLESSITÀ CICLOMATICA FINALE: 14
    // Questo significa 14 percorsi indipendenti nel codice.
    // Per testare completamente questo metodo servono ALMENO 14 test case.
}
