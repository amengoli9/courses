namespace Exercise6_Combined;

/// <summary>
/// Exercise 6: Combined SOLID Challenge
/// Run this program to test your complete North Pole Gift Delivery System.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🎄 Exercise 6: Combined SOLID Challenge 🎄");
        Console.WriteLine("===========================================\n");

        Console.WriteLine("THE ULTIMATE CHRISTMAS WORKSHOP CHALLENGE!");
        Console.WriteLine("Build a complete gift delivery system following ALL SOLID principles.\n");

        // Example gift request
        var giftRequest = new GiftRequest
        {
            ChildName = "Emma",
            Age = 8,
            Address = "123 Candy Cane Lane, Winter Wonderland",
            RequestedGifts = new List<Gift>
            {
                new Gift { Name = "Teddy Bear", Category = "Plush", BuildTime = 2 },
                new Gift { Name = "Bicycle", Category = "Sports", BuildTime = 5 },
                new Gift { Name = "Art Set", Category = "Creative", BuildTime = 1 }
            },
            NiceScore = 95 // Out of 100
        };

        Console.WriteLine("SAMPLE GIFT REQUEST:");
        Console.WriteLine($"Child: {giftRequest.ChildName}");
        Console.WriteLine($"Age: {giftRequest.Age}");
        Console.WriteLine($"Address: {giftRequest.Address}");
        Console.WriteLine($"Nice Score: {giftRequest.NiceScore}/100");
        Console.WriteLine("\nRequested Gifts:");
        foreach (var gift in giftRequest.RequestedGifts)
        {
            Console.WriteLine($"  - {gift.Name} ({gift.Category}) - {gift.BuildTime} elf-hours");
        }

        Console.WriteLine("\n========================================");
        Console.WriteLine("YOUR TASK:");
        Console.WriteLine("========================================");
        Console.WriteLine("Design and implement a complete system that can:");
        Console.WriteLine("\n1. Apply discount strategies based on nice score");
        Console.WriteLine("   - NiceListBonusDiscount (20% for score > 90)");
        Console.WriteLine("   - SiblingBundleDiscount");
        Console.WriteLine("   - EarlyRequestDiscount");
        Console.WriteLine("   - ReusedToyCreditDiscount");

        Console.WriteLine("\n2. Choose wrapping methods");
        Console.WriteLine("   - TraditionalPaperWrapping");
        Console.WriteLine("   - RecycledBrownPaperWrapping");
        Console.WriteLine("   - MagicalInvisibleWrapping");
        Console.WriteLine("   - GoldFoilDeluxeWrapping");

        Console.WriteLine("\n3. Select delivery methods");
        Console.WriteLine("   - ClassicSleighDelivery");
        Console.WriteLine("   - TurboRudolphExpress");
        Console.WriteLine("   - ChimneyTeleportation");
        Console.WriteLine("   - ElfDroneDelivery");
        Console.WriteLine("   - MagicTrainDelivery");

        Console.WriteLine("\n4. Send notifications via channels");
        Console.WriteLine("   - ChimneyLetterNotification");
        Console.WriteLine("   - DreamMessageNotification");
        Console.WriteLine("   - ParentEmailNotification");
        Console.WriteLine("   - CookieMessageNotification");
        Console.WriteLine("   - NorthPoleTextNotification");

        Console.WriteLine("\n5. Track production in workshop");
        Console.WriteLine("   - Calculate build time");
        Console.WriteLine("   - Assign elves");
        Console.WriteLine("   - Monitor capacity");

        Console.WriteLine("\n========================================");
        Console.WriteLine("EXAMPLE USAGE (after implementation):");
        Console.WriteLine("========================================");
        Console.WriteLine(@"
var processor = new NorthPoleGiftProcessor(
    new NiceListBonusDiscount(),
    new RecycledPaperWrapping(),
    new TurboRudolphExpress(),
    new DreamMessageNotification(),
    new WorkshopProductionTracker()
);

processor.ProcessGiftRequest(giftRequest);
        ");

        Console.WriteLine("\n========================================");
        Console.WriteLine("SOLID PRINCIPLES CHECKLIST:");
        Console.WriteLine("========================================");
        Console.WriteLine("[ ] SRP: Each class has ONE responsibility");
        Console.WriteLine("[ ] OCP: Can add new strategies without modification");
        Console.WriteLine("[ ] LSP: All implementations are substitutable");
        Console.WriteLine("[ ] ISP: Focused interfaces, no fat interfaces");
        Console.WriteLine("[ ] DIP: Depend on abstractions, inject dependencies");

        Console.WriteLine("\n🎅 This is the ultimate test! Good luck, elf developer! 🎅");
    }
}
