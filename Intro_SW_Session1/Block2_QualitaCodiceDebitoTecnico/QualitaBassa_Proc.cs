// ===================================================================
// BLOCCO 2 — Qualità del Codice e Debito Tecnico
// Sezione 2.1 — Cosa rende il codice "di qualità"
//
// VERSIONE A — Codice "funzionante" ma di bassa qualità
// Domanda: chi riesce a dirmi cosa fa questo codice?
// ===================================================================

namespace Intro_SW_Session1.Block2_QualitaCodiceDebitoTecnico;

public class Proc
{
    public double Calc(List<double> d, int t)
    {
        double r = 0;
        for (int i = 0; i < d.Count; i++)
        {
            if (t == 1)
            {
                if (d[i] > 0)
                    r += d[i];
            }
            else if (t == 2)
            {
                r += d[i];
            }
            else if (t == 3)
            {
                if (d[i] < 0)
                    r += d[i];
            }
        }
        if (t == 2)
            r = r / d.Count;
        return r;
    }
}
