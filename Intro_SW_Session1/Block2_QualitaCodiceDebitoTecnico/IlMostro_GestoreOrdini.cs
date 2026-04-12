// ===================================================================
// BLOCCO 2 — Qualità del Codice e Debito Tecnico
// Sezione 2.2 — Il Debito Tecnico
//
// IL MOSTRO — Debito tecnico NON gestito
// Questo è il genere di codice che nasce quando il debito tecnico
// si accumula senza essere gestito. Ogni "pezza" aggiunge complessità.
//
// Domanda: quante righe di codice dovete toccare per aggiungere un
// nuovo tipo di cliente — diciamo 'Platinum' — con le sue regole?
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block2_QualitaCodiceDebitoTecnico;

public class GestoreOrdini
{
    public string ProcessaOrdine(
        string tipoCliente,
        List<Prodotto> prodotti,
        string codicePromo,
        bool isVip,
        string paese,
        bool isBlackFriday)
    {
        double totale = 0;
        double sconto = 0;
        double tasse = 0;
        double spedizione = 0;
        string risultato = "";

        // Calcolo totale
        for (int i = 0; i < prodotti.Count; i++)
        {
            totale += prodotti[i].Prezzo * prodotti[i].Quantita;
        }

        // Calcolo sconto
        if (tipoCliente == "Gold")
        {
            sconto = totale * 0.15;
            if (isBlackFriday)
            {
                sconto = totale * 0.25; // Gold + BlackFriday
                if (codicePromo == "EXTRA10")
                {
                    sconto = totale * 0.30; // Gold + BlackFriday + Promo
                }
            }
            else if (codicePromo == "EXTRA10")
            {
                sconto = totale * 0.20; // Gold + Promo ma non BlackFriday
            }
        }
        else if (tipoCliente == "Silver")
        {
            sconto = totale * 0.10;
            if (isBlackFriday)
            {
                sconto = totale * 0.20;
                if (codicePromo == "EXTRA10")
                {
                    sconto = totale * 0.25;
                }
            }
            else if (codicePromo == "EXTRA10")
            {
                sconto = totale * 0.15;
            }
        }
        else if (tipoCliente == "Bronze")
        {
            sconto = totale * 0.05;
            if (isBlackFriday)
            {
                sconto = totale * 0.15;
                if (codicePromo == "EXTRA10")
                {
                    sconto = totale * 0.20;
                }
            }
            else if (codicePromo == "EXTRA10")
            {
                sconto = totale * 0.10;
            }
        }
        else
        {
            sconto = 0;
            if (isBlackFriday)
            {
                sconto = totale * 0.10;
                if (codicePromo == "EXTRA10")
                {
                    sconto = totale * 0.15;
                }
            }
            else if (codicePromo == "EXTRA10")
            {
                sconto = totale * 0.05;
            }
        }

        if (isVip)
        {
            sconto += totale * 0.05; // VIP bonus aggiuntivo
        }

        // Calcolo tasse per paese
        if (paese == "IT")
        {
            tasse = (totale - sconto) * 0.22;
        }
        else if (paese == "DE")
        {
            tasse = (totale - sconto) * 0.19;
        }
        else if (paese == "FR")
        {
            tasse = (totale - sconto) * 0.20;
        }
        else if (paese == "UK")
        {
            tasse = (totale - sconto) * 0.20;
        }
        else if (paese == "US")
        {
            tasse = (totale - sconto) * 0.0; // no federal sales tax
        }
        else
        {
            tasse = (totale - sconto) * 0.22; // default Italia
        }

        // Calcolo spedizione
        if (paese == "IT")
        {
            if (totale - sconto > 100)
                spedizione = 0;
            else
                spedizione = 7.99;
        }
        else if (paese == "DE" || paese == "FR")
        {
            if (totale - sconto > 150)
                spedizione = 0;
            else
                spedizione = 12.99;
        }
        else
        {
            spedizione = 19.99;
        }

        // Generazione risultato
        risultato = "ORDINE PROCESSATO\n";
        risultato += "Subtotale: " + totale.ToString("C") + "\n";
        risultato += "Sconto (" + tipoCliente + "): -" + sconto.ToString("C") + "\n";
        risultato += "Tasse (" + paese + "): " + tasse.ToString("C") + "\n";
        risultato += "Spedizione: " + spedizione.ToString("C") + "\n";
        risultato += "TOTALE: " + (totale - sconto + tasse + spedizione).ToString("C");

        return risultato;
    }
}
