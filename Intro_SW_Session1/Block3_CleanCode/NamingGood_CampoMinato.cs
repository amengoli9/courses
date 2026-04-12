// ===================================================================
// BLOCCO 3 — Clean Code: I Principi di Robert C. Martin
// Sezione 3.1 — Naming: Il principio più importante
//
// ✅ VERSIONE BUONA — I nomi raccontano la storia
// Il contesto è un campo minato (Minesweeper).
// Ora è chiaro: stiamo cercando le celle segnate con bandierina.
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block3_CleanCode;

public class CampoMinato
{
    private List<Cella> celle = new List<Cella>();

    // Ora è chiaro: stiamo cercando le celle segnate con bandierina
    public List<Cella> GetCelleSegnate()
    {
        var celleSegnate = new List<Cella>();
        foreach (var cella in celle)
        {
            if (cella.IsSegnata)
                celleSegnate.Add(cella);
        }
        return celleSegnate;
    }

    // Oppure, ancora più chiaro con LINQ:
    public List<Cella> GetCelleSegnateLinq()
    {
        return celle.Where(c => c.IsSegnata).ToList();
    }
}
