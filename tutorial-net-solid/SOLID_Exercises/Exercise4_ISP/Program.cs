namespace Exercise4_ISP;

/// <summary>
/// Exercise 4: Interface Segregation Principle
/// Run this program to see the fat interface problem and test your solution.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🧝 Exercise 4: Interface Segregation Principle 🧝");
        Console.WriteLine("==================================================\n");

        Console.WriteLine("Testing the PROBLEM code (violates ISP):");

        // Test ToyMakerElf
        Console.WriteLine("\n1. Toy Maker Elf:");
        var toyMaker = new ToyMakerElf();
        toyMaker.MakeToys();      // Works
        toyMaker.WrapGifts();     // Works
        try
        {
            toyMaker.FeedReindeer(); // Throws!
        }
        catch (NotSupportedException ex)
        {
            Console.WriteLine($"   ✗ {ex.Message}");
        }

        // Test ReindeerCaretaker
        Console.WriteLine("\n2. Reindeer Caretaker:");
        var caretaker = new ReindeerCaretaker();
        caretaker.FeedReindeer(); // Works
        caretaker.PolishSleigh(); // Works
        try
        {
            caretaker.MakeToys();    // Throws!
        }
        catch (NotSupportedException ex)
        {
            Console.WriteLine($"   ✗ {ex.Message}");
        }

        // Test Santa
        Console.WriteLine("\n3. Santa:");
        var santa = new Santa();
        santa.UpdateNaughtyNiceList(); // Works
        santa.BakeCookies();           // Works (taste testing!)
        try
        {
            santa.MakeToys();            // Throws!
        }
        catch (NotSupportedException ex)
        {
            Console.WriteLine($"   ✗ {ex.Message}");
        }

        Console.WriteLine("\n========================================");
        Console.WriteLine("PROBLEM IDENTIFIED:");
        Console.WriteLine("========================================");
        Console.WriteLine("✗ Fat interface forces implementation of unused methods");
        Console.WriteLine("✗ Leads to NotSupportedException everywhere");
        Console.WriteLine("✗ Violates ISP - clients depend on methods they don't use");

        Console.WriteLine("\n========================================");
        Console.WriteLine("YOUR TASK:");
        Console.WriteLine("========================================");
        Console.WriteLine("1. Break IWorkshopWorker into focused interfaces:");
        Console.WriteLine("   - IToyMaker");
        Console.WriteLine("   - IReindeerCaretaker");
        Console.WriteLine("   - ICookieBaker");
        Console.WriteLine("   - ISleighMechanic");
        Console.WriteLine("   - IGiftWrapper");
        Console.WriteLine("   - IListManager");
        Console.WriteLine("2. Workers implement ONLY what they need");
        Console.WriteLine("3. No more NotSupportedException!");
        Console.WriteLine("\nFollow ISP: Clients shouldn't depend on unused interfaces!");
        Console.WriteLine("\n🎅 Good luck, elf developer! 🎅");
    }
}
