namespace Exercise5_DIP;

/// <summary>
/// Exercise 5: Dependency Inversion Principle
/// Run this program to see tight coupling and test your DIP solution.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("📜 Exercise 5: Dependency Inversion Principle 📜");
        Console.WriteLine("=================================================\n");

        Console.WriteLine("Testing the PROBLEM code (violates DIP):");
        var generator = new SantaLetterGenerator();

        Console.WriteLine("\n1. Parchment Scroll:");
        generator.GenerateLetterToChild("NiceList", "Emma", "You made the nice list!");

        Console.WriteLine("\n2. Fancy Letter:");
        generator.GenerateLetterToChild("PersonalLetter", "Oliver", "Dear Oliver, keep up the good work!");

        Console.WriteLine("\n3. Chimney Email:");
        generator.GenerateLetterToChild("EmailToParents", "Sophia", "Your child has been very nice this year!");

        Console.WriteLine("\n========================================");
        Console.WriteLine("PROBLEMS IDENTIFIED:");
        Console.WriteLine("========================================");
        Console.WriteLine("✗ High-level SantaLetterGenerator depends on low-level writers");
        Console.WriteLine("✗ Tightly coupled - creates instances with 'new' keyword");
        Console.WriteLine("✗ Hard to test - can't inject mock writers");
        Console.WriteLine("✗ Hard to extend - must modify if-else chain for new formats");
        Console.WriteLine("✗ Violates DIP - depends on concretions, not abstractions");

        Console.WriteLine("\n========================================");
        Console.WriteLine("YOUR TASK:");
        Console.WriteLine("========================================");
        Console.WriteLine("1. Create IChristmasLetterWriter interface");
        Console.WriteLine("2. Make all writers implement the interface");
        Console.WriteLine("3. Inject writer dependency via constructor");
        Console.WriteLine("4. Remove if-else chain and string-based selection");
        Console.WriteLine("5. Add a new HolographicLetterWriter without modifying existing code");
        Console.WriteLine("\nFollow DIP: High-level modules should not depend on low-level modules.");
        Console.WriteLine("Both should depend on abstractions!");

        Console.WriteLine("\n========================================");
        Console.WriteLine("EXAMPLE AFTER REFACTORING:");
        Console.WriteLine("========================================");
        Console.WriteLine("var writer = new ParchmentScrollWriter();");
        Console.WriteLine("var generator = new ImprovedSantaLetterGenerator(writer);");
        Console.WriteLine("generator.GenerateLetterToChild(\"Emma\", \"You're on the nice list!\");");
        Console.WriteLine("\n// Easy to test:");
        Console.WriteLine("var mockWriter = new MockLetterWriter();");
        Console.WriteLine("var testGenerator = new ImprovedSantaLetterGenerator(mockWriter);");

        Console.WriteLine("\n🎅 Good luck, elf developer! 🎅");
    }
}
