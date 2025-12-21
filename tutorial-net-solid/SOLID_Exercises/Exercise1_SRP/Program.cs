namespace Exercise1_SRP;

/// <summary>
/// Exercise 1: Single Responsibility Principle
/// Run this program to test the original problem code and your refactored solution.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🎄 Exercise 1: Single Responsibility Principle 🎄");
        Console.WriteLine("================================================\n");

        // Test the problematic ToyRequestManager
        Console.WriteLine("Testing the PROBLEM code (violates SRP):");
        Console.WriteLine("This class does too many things!\n");

        try
        {
            var manager = new ToyRequestManager();
            // Uncomment to test (will fail without actual database):
            // manager.ProcessToyRequest("Emma", "123 Candy Cane Lane", "Teddy Bear", 8);
            Console.WriteLine("✗ This class has multiple responsibilities!");
            Console.WriteLine("  - Validation");
            Console.WriteLine("  - Inventory checking");
            Console.WriteLine("  - Database operations");
            Console.WriteLine("  - Email sending");
            Console.WriteLine("  - Logging");
            Console.WriteLine("  - Queue management");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\n========================================");
        Console.WriteLine("YOUR TASK:");
        Console.WriteLine("========================================");
        Console.WriteLine("1. Identify all responsibilities (listed above)");
        Console.WriteLine("2. Create separate classes for each responsibility");
        Console.WriteLine("3. Create a coordinator that uses all specialists");
        Console.WriteLine("\nFollow the SRP: Each class should have ONE reason to change!");
        Console.WriteLine("\n🎅 Good luck, elf developer! 🎅");
    }
}
