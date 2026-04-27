// ISP — CORRETTO: interfacce a ruolo, ognuna dichiara una sola responsabilità.
// Ogni classe implementa solo quello che davvero sa fare.
// I consumatori dipendono solo dall'interfaccia di cui hanno bisogno.

public interface IReadable      { void Open(string path); }
public interface IWritable      { void Save(string path); }
public interface IPrintable     { void Print(); }
public interface IEmailable     { void Email(string to); }
public interface ISignable      { void Sign(Certificate cert); }
public interface IEncryptable   { void Encrypt(string password); }
public interface IPdfConvertible { void ConvertToPdf(string outputPath); }
public interface IOcrScannable  { void OcrScan(); }
public interface ITranslatable  { void TranslateTo(string language); }

// PlainTextEditor implementa solo quello che ha senso
public class PlainTextEditor : IReadable, IWritable, IPrintable
{
    public void Open(string path) { /* ... */ }
    public void Save(string path) { /* ... */ }
    public void Print() { /* ... */ }
}

// PdfDocument supporta un set più ricco
public class PdfDocument : IReadable, IWritable, IPrintable, IEmailable, ISignable, IEncryptable, IPdfConvertible
{
    public void Open(string path)       { /* ... */ }
    public void Save(string path)       { /* ... */ }
    public void Print()                 { /* ... */ }
    public void Email(string to)        { /* ... */ }
    public void Sign(Certificate cert)  { /* ... */ }
    public void Encrypt(string password){ /* ... */ }
    public void ConvertToPdf(string o)  { /* ... */ }
}

// ScannedImage: solo le operazioni pertinenti
public class ScannedImage : IReadable, IPrintable, IOcrScannable
{
    public void Open(string path) { /* ... */ }
    public void Print()           { /* ... */ }
    public void OcrScan()         { /* ... */ }
}

// I consumatori dichiarano solo il ruolo che usano
public class PrintQueue
{
    public void Add(IPrintable item) { /* ... */ }
    // Non sa nulla di Email, Sign, Encrypt
}

public class DigitalSigner
{
    public void Sign(ISignable doc, Certificate cert) { /* ... */ }
    // Non sa nulla di Open, Save, Print
}
