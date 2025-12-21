using System;
namespace Exercise3B_LSP;

// Classe base
public class Rettangolo
{
    public virtual int Larghezza { get; set; }
    public virtual int Altezza { get; set; }

    public int CalcolaArea()
    {
        return Larghezza * Altezza;
    }
}

// Classe derivata che VIOLA LSP
public class Quadrato : Rettangolo
{
    public override int Larghezza
    {
        get => base.Larghezza;
        set
        {
            base.Larghezza = value;
            base.Altezza = value; // Imposta anche l'altezza!
        }
    }

    public override int Altezza
    {
        get => base.Altezza;
        set
        {
            base.Altezza = value;
            base.Larghezza = value; // Imposta anche la larghezza!
        }
    }
}

// Codice che usa queste classi
class Program
{
    static void Main()
    {
        Rettangolo rett = new Rettangolo();
        TestRettangolo(rett); // Funziona: stampa 50

        Rettangolo quadrato = new Quadrato();
        TestRettangolo(quadrato); // FALLISCE: stampa 25 invece di 50!
    }

    static void TestRettangolo(Rettangolo r)
    {
        r.Altezza = 10;
        r.Larghezza = 5;

        Console.WriteLine($"Area attesa: 50");
        Console.WriteLine($"Area ottenuta: {r.CalcolaArea()}");

        // Con Rettangolo: 5 * 10 = 50 ✓
        // Con Quadrato: 10 * 10 = 25 ✗ (perché impostando Altezza=10, 
        //                                 anche Larghezza diventa 10)
    }
}