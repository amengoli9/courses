// ISP — VIOLAZIONE: interfaccia "grassa" con 9 metodi.
// PlainTextEditor è costretto a fingere di supportare Email, Sign, Encrypt, ecc.
// Le NotSupportedException sono il sintomo classico di ISP violato (e di LSP violato).

public interface IDocumentService
{
    void Open(string path);
    void Save(string path);
    void Print();
    void Email(string to);
    void Sign(Certificate cert);
    void Encrypt(string password);
    void ConvertToPdf(string outputPath);
    void OcrScan();
    void TranslateTo(string language);
}

public class PlainTextEditor : IDocumentService
{
    public void Open(string path)              { /* implementato */ }
    public void Save(string path)              { /* implementato */ }
    public void Print()                        { /* implementato */ }
    public void Email(string to)               => throw new NotSupportedException();
    public void Sign(Certificate c)            => throw new NotSupportedException();
    public void Encrypt(string p)              => throw new NotSupportedException();
    public void ConvertToPdf(string output)    => throw new NotSupportedException();
    public void OcrScan()                      => throw new NotSupportedException();
    public void TranslateTo(string language)   => throw new NotSupportedException();
}

// Problema aggiuntivo: se aggiungiamo Watermark() a IDocumentService,
// PlainTextEditor DEVE essere modificato, anche se non lo supporterà mai.
