namespace Exercise5_DIP;

/// <summary>
/// Exercise 5: Dependency Inversion Principle (DIP)
/// Santa's Letter Generator Problem
///
/// PROBLEM: The high-level letter generator is tightly coupled to low-level
/// concrete implementations, making it impossible to test and extend.
///
/// YOUR TASK:
/// 1. Create an abstraction (interface) for letter writers
/// 2. Refactor SantaLetterGenerator to depend on the abstraction
/// 3. Inject dependencies through the constructor
/// 4. Make it easy to add new letter formats
///
/// PRINCIPLE: "Depend on abstractions, not concretions"
/// </summary>
public class SantaLetterGenerator
{
    public void GenerateLetterToChild(string letterType, string childName, string content)
    {
        // Tightly coupled to concrete classes
        if (letterType == "NiceList")
        {
            var scrollWriter = new ParchmentScrollWriter();
            scrollWriter.Write(content, $"{childName}_nice_certificate.scroll");
        }
        else if (letterType == "PersonalLetter")
        {
            var fancyWriter = new FancyLetterWriter();
            fancyWriter.Write(content, $"{childName}_letter.fancy");
        }
        else if (letterType == "EmailToParents")
        {
            var emailWriter = new ChimneyEmailWriter();
            emailWriter.Write(content, $"{childName}_update.email");
        }
        else if (letterType == "CertificateOfNiceness")
        {
            var certificateWriter = new GoldFoilCertificateWriter();
            certificateWriter.Write(content, $"{childName}_certificate.pdf");
        }
    }
}

public class ParchmentScrollWriter
{
    public void Write(string content, string filename)
    {
        Console.WriteLine($"Writing on magical parchment scroll: {filename}");
        Console.WriteLine($"Content: {content}");
    }
}

public class FancyLetterWriter
{
    public void Write(string content, string filename)
    {
        Console.WriteLine($"Writing fancy letter with North Pole seal: {filename}");
        Console.WriteLine($"Content: {content}");
    }
}

public class ChimneyEmailWriter
{
    public void Write(string content, string filename)
    {
        Console.WriteLine($"Sending via chimney email network: {filename}");
        Console.WriteLine($"Content: {content}");
    }
}

public class GoldFoilCertificateWriter
{
    public void Write(string content, string filename)
    {
        Console.WriteLine($"Creating gold foil certificate: {filename}");
        Console.WriteLine($"Content: {content}");
    }
}

/*
 * ========================================
 * YOUR SOLUTION GOES BELOW THIS LINE
 * ========================================
 *
 * Apply Dependency Inversion:
 *
 * 1. Create IChristmasLetterWriter interface:
 *    - void WriteChristmasLetter(string content, string recipientName)
 *    - string FormatDescription { get; }
 *
 * 2. Make all writers implement the interface:
 *    - ParchmentScrollWriter : IChristmasLetterWriter
 *    - FancyLetterWriter : IChristmasLetterWriter
 *    - ChimneyEmailWriter : IChristmasLetterWriter
 *    - GoldFoilCertificateWriter : IChristmasLetterWriter
 *    - HolographicLetterWriter : IChristmasLetterWriter (NEW!)
 *
 * 3. Refactor SantaLetterGenerator:
 *    - Accept IChristmasLetterWriter in constructor
 *    - Remove if-else chain
 *    - Depend on abstraction, not concrete classes
 *
 * Example usage after refactoring:
 *   var generator = new ImprovedSantaLetterGenerator(new ParchmentScrollWriter());
 *   generator.GenerateLetterToChild("Emma", "You've been very nice!");
 *
 * Benefits:
 * - Easy to test with mock letter writers
 * - Easy to add new letter formats
 * - High-level code doesn't depend on low-level details
 */

// TODO: Create IChristmasLetterWriter interface

// TODO: Refactor writers to implement interface

// TODO: Create ImprovedSantaLetterGenerator using DIP
