namespace Exercise3B_LSP.Solution;

// Interfaccia comune
public interface IForma
{
    int CalcolaArea();
}

// Implementazioni indipendenti
public class Rettangolo : IForma
{
    public int Larghezza { get; set; }
    public int Altezza { get; set; }

    public int CalcolaArea() => Larghezza * Altezza;
}

public class Quadrato : IForma
{
    public int Lato { get; set; }

    public int CalcolaArea() => Lato * Lato;
}

// Ora il codice lavora con l'interfaccia
public static class FormaTester
{
    public static void TestForma(IForma forma)
    {
        Console.WriteLine($"Area: {forma.CalcolaArea()}");
    }
}

//FormaTester.TestForma(new Rettangolo { Larghezza = 5, Altezza = 10 }); // Stampa 50
//FormaTester.TestForma(new Quadrato { Lato = 10 }); // Stampa 100
