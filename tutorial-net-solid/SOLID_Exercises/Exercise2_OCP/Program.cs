namespace Exercise2_OCP;

/// <summary>
/// Exercise 2: Open/Closed Principle
/// Run this program to test your refactored solution using the Strategy pattern.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🦌 Exercise 2: Open/Closed Principle 🦌");
        Console.WriteLine("========================================\n");

        Console.WriteLine("Testing the PROBLEM code (violates OCP):");
        var oldCalculator = new GiftDeliveryCalculator();

        Console.WriteLine($"Classic Sleigh (1000 miles): {oldCalculator.CalculateDeliveryTime(1000, "ClassicSleigh")} minutes");
        Console.WriteLine($"Turbo Reindeer (1000 miles): {oldCalculator.CalculateDeliveryTime(1000, "TurboReindeer")} minutes");
        Console.WriteLine($"Magic Teleport (1000 miles): {oldCalculator.CalculateDeliveryTime(1000, "MagicTeleport")} minutes");
        Console.WriteLine($"Drone Elf (1000 miles): {oldCalculator.CalculateDeliveryTime(1000, "DroneElf")} minutes");

        Console.WriteLine("\n✗ PROBLEM: To add a new delivery method, we must modify this class!");
        Console.WriteLine("✗ This violates OCP - we're not closed for modification");

        Console.WriteLine("\n========================================");
        Console.WriteLine("YOUR TASK:");
        Console.WriteLine("========================================");
        Console.WriteLine("1. Create IGiftDeliveryStrategy interface");
        Console.WriteLine("2. Implement 5+ concrete delivery strategies");
        Console.WriteLine("3. Refactor calculator to accept strategies");
        Console.WriteLine("4. Add NEW delivery methods without modifying existing code");
        Console.WriteLine("\nFollow OCP: Open for extension, closed for modification!");

        Console.WriteLine("\n========================================");
        Console.WriteLine("EXAMPLE USAGE (after refactoring):");
        Console.WriteLine("========================================");
        Console.WriteLine("var strategy = new ClassicSleighDelivery();");
        Console.WriteLine("var calculator = new ImprovedGiftDeliveryCalculator(strategy);");
        Console.WriteLine("var time = calculator.CalculateDeliveryTime(1000);");
        Console.WriteLine("\n🎅 Good luck, elf developer! 🎅");
    }
}
