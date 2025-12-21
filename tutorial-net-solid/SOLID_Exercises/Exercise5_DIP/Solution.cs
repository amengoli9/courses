namespace Exercise5_DIP.Solution;

/// <summary>
/// SOLUTION: Dependency Inversion Principle
/// High-level modules should not depend on low-level modules.
/// Both should depend on abstractions.
///
/// Abstractions should not depend on details.
/// Details should depend on abstractions.
/// </summary>

// ========================================
// ABSTRACTION - Define the contract (depends on nothing)
// ========================================
public interface IChristmasLetterWriter
{
    void WriteChristmasLetter(string content, string recipientName);
    string FormatDescription { get; }
}

// ========================================
// LOW-LEVEL MODULES - Concrete implementations (depend on abstraction)
// ========================================

public class ParchmentScrollWriter : IChristmasLetterWriter
{
    public string FormatDescription => "Magical Parchment Scroll";

    public void WriteChristmasLetter(string content, string recipientName)
    {
        Console.WriteLine($"📜 Writing on magical parchment scroll for {recipientName}");
        Console.WriteLine($"   Using ancient elf calligraphy...");
        Console.WriteLine($"   Content: {content}");
        Console.WriteLine($"   Sealing with North Pole wax seal ❄️");
    }
}

public class FancyLetterWriter : IChristmasLetterWriter
{
    public string FormatDescription => "Fancy Letter with Wax Seal";

    public void WriteChristmasLetter(string content, string recipientName)
    {
        Console.WriteLine($"✉️ Writing fancy letter for {recipientName}");
        Console.WriteLine($"   Using premium North Pole stationary...");
        Console.WriteLine($"   Content: {content}");
        Console.WriteLine($"   Adding red wax seal with Santa's stamp 🎅");
    }
}

public class ChimneyEmailWriter : IChristmasLetterWriter
{
    public string FormatDescription => "Chimney Email Network";

    public void WriteChristmasLetter(string content, string recipientName)
    {
        Console.WriteLine($"📧 Sending chimney email to {recipientName}");
        Console.WriteLine($"   Connecting to chimney.northpole.christmas...");
        Console.WriteLine($"   Content: {content}");
        Console.WriteLine($"   Delivered via magical chimney network! 🏠");
    }
}

public class GoldFoilCertificateWriter : IChristmasLetterWriter
{
    public string FormatDescription => "Gold Foil Certificate";

    public void WriteChristmasLetter(string content, string recipientName)
    {
        Console.WriteLine($"🏆 Creating gold foil certificate for {recipientName}");
        Console.WriteLine($"   Using premium 24k gold foil...");
        Console.WriteLine($"   Content: {content}");
        Console.WriteLine($"   Embossing with official North Pole seal ✨");
    }
}

public class HolographicLetterWriter : IChristmasLetterWriter
{
    public string FormatDescription => "3D Holographic Message";

    public void WriteChristmasLetter(string content, string recipientName)
    {
        Console.WriteLine($"🌟 Generating 3D holographic message for {recipientName}");
        Console.WriteLine($"   Initializing hologram projectors...");
        Console.WriteLine($"   Content: {content}");
        Console.WriteLine($"   Hologram will appear at midnight! ✨");
    }
}

public class DreamMessageWriter : IChristmasLetterWriter
{
    public string FormatDescription => "Dream Message Delivery";

    public void WriteChristmasLetter(string content, string recipientName)
    {
        Console.WriteLine($"💭 Preparing dream message for {recipientName}");
        Console.WriteLine($"   Encoding message into dream frequency...");
        Console.WriteLine($"   Content: {content}");
        Console.WriteLine($"   Will appear in dreams tonight! 😴");
    }
}

public class CookieMessageWriter : IChristmasLetterWriter
{
    public string FormatDescription => "Frosted Cookie Message";

    public void WriteChristmasLetter(string content, string recipientName)
    {
        Console.WriteLine($"🍪 Writing message in frosting for {recipientName}");
        Console.WriteLine($"   Baking special message cookie...");
        Console.WriteLine($"   Content: {content}");
        Console.WriteLine($"   Decorated with festive icing! 🎨");
    }
}

// ========================================
// HIGH-LEVEL MODULE - Depends on abstraction (not concretions)
// ========================================
public class ImprovedSantaLetterGenerator
{
    private readonly IChristmasLetterWriter _letterWriter;

    // Dependency is INJECTED via constructor
    public ImprovedSantaLetterGenerator(IChristmasLetterWriter letterWriter)
    {
        _letterWriter = letterWriter ?? throw new ArgumentNullException(nameof(letterWriter));
    }

    public void GenerateLetterToChild(string childName, string content)
    {
        Console.WriteLine($"\n🎅 Generating letter for {childName}");
        Console.WriteLine($"   Format: {_letterWriter.FormatDescription}");
        Console.WriteLine();

        _letterWriter.WriteChristmasLetter(content, childName);

        Console.WriteLine($"\n✅ Letter sent to {childName}!\n");
    }

    public void GenerateMultipleLetters(List<(string name, string content)> letters)
    {
        foreach (var (name, content) in letters)
        {
            GenerateLetterToChild(name, content);
        }
    }
}

// ========================================
// FACTORY PATTERN (optional) - For managing writers
// ========================================
public class LetterWriterFactory
{
    private readonly Dictionary<string, Func<IChristmasLetterWriter>> _writers;

    public LetterWriterFactory()
    {
        _writers = new Dictionary<string, Func<IChristmasLetterWriter>>
        {
            { "parchment", () => new ParchmentScrollWriter() },
            { "fancy", () => new FancyLetterWriter() },
            { "email", () => new ChimneyEmailWriter() },
            { "certificate", () => new GoldFoilCertificateWriter() },
            { "hologram", () => new HolographicLetterWriter() },
            { "dream", () => new DreamMessageWriter() },
            { "cookie", () => new CookieMessageWriter() }
        };
    }

    public IChristmasLetterWriter CreateWriter(string type)
    {
        if (_writers.TryGetValue(type.ToLower(), out var factory))
        {
            return factory();
        }

        // Default to parchment scroll
        return new ParchmentScrollWriter();
    }

    public IEnumerable<string> GetAvailableFormats()
    {
        return _writers.Keys;
    }
}

// ========================================
// TESTING - Mock implementation for unit tests
// ========================================
public class MockLetterWriter : IChristmasLetterWriter
{
    public string FormatDescription => "Mock Letter Writer (for testing)";
    public List<(string content, string recipient)> SentLetters { get; } = new();

    public void WriteChristmasLetter(string content, string recipientName)
    {
        SentLetters.Add((content, recipientName));
        Console.WriteLine($"[MOCK] Letter recorded: {recipientName}");
    }
}

// ========================================
// DEMONSTRATION
// ========================================
public class DipSolutionDemo
{
    public static void Run()
    {
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("SOLUTION: Dependency Inversion Principle");
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine();

        // Example 1: Using different letter writers
        Console.WriteLine("EXAMPLE 1: Using Different Letter Writers");
        Console.WriteLine("-" .PadRight(60, '-'));

        var letters = new List<(string name, string content)>
        {
            ("Emma", "You've been very nice this year!"),
            ("Oliver", "Keep up the good work!"),
            ("Sophia", "Your kindness has not gone unnoticed!")
        };

        // Same high-level code, different implementations!
        var parchmentGenerator = new ImprovedSantaLetterGenerator(new ParchmentScrollWriter());
        parchmentGenerator.GenerateLetterToChild("Emma", "You've been very nice this year!");

        var emailGenerator = new ImprovedSantaLetterGenerator(new ChimneyEmailWriter());
        emailGenerator.GenerateLetterToChild("Oliver", "Keep up the good work!");

        var hologramGenerator = new ImprovedSantaLetterGenerator(new HolographicLetterWriter());
        hologramGenerator.GenerateLetterToChild("Sophia", "Your kindness has not gone unnoticed!");

        // Example 2: Using factory
        Console.WriteLine("\n" + "=" .PadRight(60, '='));
        Console.WriteLine("EXAMPLE 2: Using Factory Pattern");
        Console.WriteLine("-" .PadRight(60, '-'));

        var factory = new LetterWriterFactory();

        Console.WriteLine("\nAvailable formats:");
        foreach (var format in factory.GetAvailableFormats())
        {
            Console.WriteLine($"  - {format}");
        }

        var writer = factory.CreateWriter("dream");
        var dreamGenerator = new ImprovedSantaLetterGenerator(writer);
        dreamGenerator.GenerateLetterToChild("Luna", "Sweet dreams and Merry Christmas!");

        // Example 3: Testing with mock
        Console.WriteLine("\n" + "=" .PadRight(60, '='));
        Console.WriteLine("EXAMPLE 3: Unit Testing with Mock");
        Console.WriteLine("-" .PadRight(60, '-'));

        var mockWriter = new MockLetterWriter();
        var testGenerator = new ImprovedSantaLetterGenerator(mockWriter);

        testGenerator.GenerateLetterToChild("TestChild1", "Test message 1");
        testGenerator.GenerateLetterToChild("TestChild2", "Test message 2");

        Console.WriteLine($"\nMock captured {mockWriter.SentLetters.Count} letters:");
        foreach (var (content, recipient) in mockWriter.SentLetters)
        {
            Console.WriteLine($"  - To: {recipient}, Content: {content}");
        }

        Console.WriteLine();
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("BENEFITS OF DIP:");
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("✓ High-level code doesn't depend on low-level details");
        Console.WriteLine("✓ Both depend on abstraction (IChristmasLetterWriter)");
        Console.WriteLine("✓ Easy to swap implementations (just inject different writer)");
        Console.WriteLine("✓ Easy to test (can inject mock writers)");
        Console.WriteLine("✓ Easy to extend (add new writers without changing generator)");
        Console.WriteLine("✓ Loose coupling between modules");
        Console.WriteLine("✓ Dependencies flow inward toward abstractions");
        Console.WriteLine();

        Console.WriteLine("KEY INSIGHTS:");
        Console.WriteLine("  1. Constructor injection makes dependencies explicit");
        Console.WriteLine("  2. Abstractions are stable; implementations can change");
        Console.WriteLine("  3. High-level policy doesn't depend on low-level details");
        Console.WriteLine("  4. Testability comes naturally with DIP");
        Console.WriteLine();

        Console.WriteLine("DEPENDENCY FLOW:");
        Console.WriteLine("  ❌ OLD: SantaLetterGenerator → ParchmentScrollWriter");
        Console.WriteLine("         (High-level depends on low-level)");
        Console.WriteLine();
        Console.WriteLine("  ✅ NEW: ImprovedSantaLetterGenerator → IChristmasLetterWriter");
        Console.WriteLine("         ParchmentScrollWriter → IChristmasLetterWriter");
        Console.WriteLine("         (Both depend on abstraction)");
        Console.WriteLine();
    }
}
