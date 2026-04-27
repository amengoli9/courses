// LSP — CORRETTO: Square e Rectangle non sono in relazione padre-figlio.
// Entrambi implementano IShape e sono trattati come "forme con area".
// Nessuno dei due rompe le aspettative dell'altro.

public interface IShape
{
    int Area { get; }
}

public class Rectangle : IShape
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int Area => Width * Height;
}

public class Square : IShape
{
    public int Side { get; set; }
    public int Area => Side * Side;
}

// ──────────────────────────────────────────────────────────────────────────────
// LSP — Altro esempio: gerarchia di uccelli
// Il problema: Penguin eredita da Bird ma non può volare → eccezione inattesa.

// ❌ VIOLAZIONE
public class BirdBad
{
    public virtual void Fly() { /* ... */ }
}

public class PenguinBad : BirdBad
{
    public override void Fly()
    {
        throw new NotSupportedException("I pinguini non volano");
    }
}

// Qualunque metodo che chiama bird.Fly() esplode con un Penguin.

// ✅ CORRETTO: la gerarchia rispecchia il comportamento, non la tassonomia
public abstract class Bird { }

public abstract class FlyingBird : Bird
{
    public abstract void Fly();
}

public class Eagle : FlyingBird
{
    public override void Fly() { /* vola */ }
}

public class Penguin : Bird
{
    // Nessun metodo Fly: il sistema non può passare un Penguin dove serve FlyingBird.
}
