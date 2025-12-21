namespace Exercise3_LSP;

/// <summary>
/// Exercise 3: Liskov Substitution Principle
/// Run this program to test the broken hierarchy and your improved solution.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🎁 Exercise 3: Liskov Substitution Principle 🎁");
        Console.WriteLine("================================================\n");

        Console.WriteLine("Testing the PROBLEM code (violates LSP):");
        var workshop = new ElfWorkshop();

        // Standard wrapper - works fine
        Console.WriteLine("\n1. Standard Gift Wrapper:");
        try
        {
            var standardWrapper = new StandardGiftWrapper();
            workshop.PrepareGift(standardWrapper, "Teddy Bear");
            Console.WriteLine("✓ Success!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error: {ex.Message}");
        }

        // Edible wrapper - throws exception on AddRibbon!
        Console.WriteLine("\n2. Edible Gift Wrapper:");
        try
        {
            var edibleWrapper = new EdibleGiftWrapper();
            workshop.PrepareGift(edibleWrapper, "Chocolate Santa");
            Console.WriteLine("✓ Success!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error: {ex.Message}");
        }

        // Invisible wrapper - throws exceptions on decorations!
        Console.WriteLine("\n3. Invisible Gift Wrapper:");
        try
        {
            var invisibleWrapper = new InvisibleGiftWrapper();
            workshop.PrepareGift(invisibleWrapper, "Surprise Gift");
            Console.WriteLine("✓ Success!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error: {ex.Message}");
        }

        Console.WriteLine("\n========================================");
        Console.WriteLine("PROBLEM IDENTIFIED:");
        Console.WriteLine("========================================");
        Console.WriteLine("✗ Not all GiftWrapper subclasses can be substituted!");
        Console.WriteLine("✗ Some throw NotSupportedException");
        Console.WriteLine("✗ Violates LSP - can't replace base with derived");

        Console.WriteLine("\n========================================");
        Console.WriteLine("YOUR TASK:");
        Console.WriteLine("========================================");
        Console.WriteLine("1. Create separate interfaces for different capabilities");
        Console.WriteLine("2. IGiftWrapper (base wrapping)");
        Console.WriteLine("3. IRibbonDecorator (optional ribbon)");
        Console.WriteLine("4. IBowDecorator (optional bow)");
        Console.WriteLine("5. Update workshop to check capabilities");
        Console.WriteLine("\nFollow LSP: Derived classes must be substitutable for base classes!");
        Console.WriteLine("\n🎅 Good luck, elf developer! 🎅");
    }
}
