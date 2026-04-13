// ===================================================================
// BLOCCO 2 — Qualità del Codice e Debito Tecnico
// Sezione 2.1 — Cosa rende il codice "di qualità"
//
// IL MOSTRO — Versione con nomi criptici
//
// Esercizio: cosa fa questo metodo?
// Cosa sono tc, pl, cp, v, p, bf?
// Quali valori può avere tc? Cosa significa s = t * 0.15?
//
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block2_QualitaCodiceDebitoTecnico;

public class OrdMgr
{
    public string Proc(
        string tc,
        List<Prodotto> pl,
        string cp,
        bool v,
        string p,
        bool bf)
    {
        double t = 0;
        double s = 0;
        double tx = 0;
        double sh = 0;
        string r = "";

        for (int i = 0; i < pl.Count; i++)
        {
            t += pl[i].Prezzo * pl[i].Quantita;
        }

        if (tc == "Gold")
        {
            s = t * 0.15;
            if (bf)
            {
                s = t * 0.25;
                if (cp == "EXTRA10")
                {
                    s = t * 0.30;
                }
            }
            else if (cp == "EXTRA10")
            {
                s = t * 0.20;
            }
        }
        else if (tc == "Silver")
        {
            s = t * 0.10;
            if (bf)
            {
                s = t * 0.20;
                if (cp == "EXTRA10")
                {
                    s = t * 0.25;
                }
            }
            else if (cp == "EXTRA10")
            {
                s = t * 0.15;
            }
        }
        else if (tc == "Bronze")
        {
            s = t * 0.05;
            if (bf)
            {
                s = t * 0.15;
                if (cp == "EXTRA10")
                {
                    s = t * 0.20;
                }
            }
            else if (cp == "EXTRA10")
            {
                s = t * 0.10;
            }
        }
        else
        {
            s = 0;
            if (bf)
            {
                s = t * 0.10;
                if (cp == "EXTRA10")
                {
                    s = t * 0.15;
                }
            }
            else if (cp == "EXTRA10")
            {
                s = t * 0.05;
            }
        }

        if (v)
        {
            s += t * 0.05;
        }

        if (p == "IT")
        {
            tx = (t - s) * 0.22;
        }
        else if (p == "DE")
        {
            tx = (t - s) * 0.19;
        }
        else if (p == "FR")
        {
            tx = (t - s) * 0.20;
        }
        else if (p == "UK")
        {
            tx = (t - s) * 0.20;
        }
        else if (p == "US")
        {
            tx = (t - s) * 0.0;
        }
        else
        {
            tx = (t - s) * 0.22;
        }

        if (p == "IT")
        {
            if (t - s > 100)
                sh = 0;
            else
                sh = 7.99;
        }
        else if (p == "DE" || p == "FR")
        {
            if (t - s > 150)
                sh = 0;
            else
                sh = 12.99;
        }
        else
        {
            sh = 19.99;
        }

        r = "ORDINE PROCESSATO\n";
        r += "Subtotale: " + t.ToString("C") + "\n";
        r += "Sconto (" + tc + "): -" + s.ToString("C") + "\n";
        r += "Tasse (" + p + "): " + tx.ToString("C") + "\n";
        r += "Spedizione: " + sh.ToString("C") + "\n";
        r += "TOTALE: " + (t - s + tx + sh).ToString("C");

        return r;
    }
}
