// ===================================================================
// BLOCCO 3 — Clean Code: I Principi di Robert C. Martin
// Sezione 3.1 — Naming: Il principio più importante
//
// ❌ VERSIONE ORRIBILE — Naming che nasconde l'intenzione
// Cosa fa questo codice? Nessuno lo sa senza leggere il corpo.
// ===================================================================

namespace Intro_SW_Session1.Block3_CleanCode;

public class Mgr
{
    private List<int[]> theList = new List<int[]>();

    // Cosa fa questo metodo? Nessuno lo sa senza leggere il corpo.
    public List<int[]> GetThem()
    {
        var list1 = new List<int[]>();
        foreach (var x in theList)
        {
            if (x[0] == 4)
                list1.Add(x);
        }
        return list1;
    }

    // E questo? 'd1' e 'd2' cosa sono?
    public int Calc(int d1, int d2, int t)
    {
        int r;
        if (t == 1)
            r = d1 + d2;
        else if (t == 2)
            r = d1 - d2;
        else
            r = d1 * d2;
        return r;
    }

    // 'proc' — processa cosa?
    public void Proc(string fn, string ln, string em, int a, string dep)
    {
        // ... 50 righe che fanno... qualcosa
    }
}
