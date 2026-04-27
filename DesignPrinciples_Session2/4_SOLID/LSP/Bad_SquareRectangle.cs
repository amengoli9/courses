// LSP — VIOLAZIONE: Square eredita da Rectangle ma rompe le sue aspettative.
// Un chiamante che riceve Rectangle si aspetta di poter impostare W e H indipendentemente.
// Con Square, impostare H cambia anche W → invariante violato.

public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    public int Area => Width * Height;
}

public class Square : Rectangle
{
    public override int Width
    {
        set { base.Width = value; base.Height = value; }
    }

    public override int Height
    {
        set { base.Width = value; base.Height = value; }
    }
}

// Il test che dimostra la violazione:
void DemostaLaViolazione()
{
    Rectangle r = new Square();
    r.Width = 5;
    r.Height = 4;

    // Un chiamante ragionevole si aspetta 20.
    // Ottiene 16 (Square forza Width==Height, l'ultimo setter vince).
    Console.WriteLine(r.Area);  // → 16, non 20 → LSP violato
}

// Radice del problema:
// La tassonomia matematica (Square IS-A Rectangle) ≠ gerarchia comportamentale.
// Rectangle promette che Width e Height sono indipendenti. Square non può mantenerlo.
