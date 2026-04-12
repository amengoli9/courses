// ===================================================================
// BLOCCO 3 — Clean Code: I Principi di Robert C. Martin
// Sezione 3.3 — Commenti: quando servono e quando sono un code smell
//
// ❌ COMMENTI CATTIVI — Ogni uno di questi ha un problema:
//    - RUMOROSO: ripete il nome del metodo
//    - COSA: ripete il codice in italiano
//    - CODICE COMMENTATO: "magari ci serve dopo"
//    - OBSOLETO: il version control lo sa già
// ===================================================================

namespace Intro_SW_Session1.Block3_CleanCode;

public class GestoreInventario
{
    private object prodotto; // stub per l'esempio

    // Metodo che gestisce l'inventario  <-- RUMOROSO: ripete il nome
    public void GestisciInventario()
    {
        // Controlla se il prodotto esiste  <-- COSA: ripete il codice
        if (prodotto != null)
        {
            // Decrementa la quantità di 1  <-- COSA: il codice dice già -1
            // prodotto.Quantita = prodotto.Quantita - 1;

            // Se la quantità è minore di 10  <-- COSA: posso leggere l'if
            // if (prodotto.Quantita < 10)
            // {
            //     // Manda notifica  <-- COSA: il metodo si chiama mandaNotifica
            //     MandaNotifica(prodotto);
            // }
        }

        // Calcola il prezzo
        // var prezzo = prodotto.Prezzo * quantita;  <-- CODICE COMMENTATO
        // prezzo = prezzo - sconto;                  <-- quando mai lo togliete?
        // return prezzo;

        // Aggiornato da Marco il 15/03  <-- OBSOLETO: il version control
        //                                   lo sa già, e Marco se n'è andato
    }

    private void MandaNotifica(object prodotto)
    {
        // stub
    }
}
