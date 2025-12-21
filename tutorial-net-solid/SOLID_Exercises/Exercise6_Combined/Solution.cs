namespace Exercise6_Combined.Solution;

/// <summary>
/// SOLUTION: Combined SOLID Challenge
/// Complete North Pole Gift Delivery System demonstrating ALL SOLID principles.
///
/// This solution shows how all five SOLID principles work together in a real system.
/// </summary>

// ========================================
// CORE ABSTRACTIONS (DIP, ISP)
// ========================================

public interface IGiftDiscountStrategy
{
    decimal CalculateDiscount(decimal totalValue, int niceScore);
    string DiscountName { get; }
}

public interface IGiftWrappingService
{
    void WrapGift(Gift gift);
    decimal WrappingCost { get; }
    string WrappingType { get; }
}

public interface IDeliveryMethod
{
    int CalculateDeliveryTime(string destination);
    decimal DeliveryCost { get; }
    string MethodName { get; }
}

public interface INotificationService
{
    void SendDeliveryNotification(GiftRequest request, DateTime deliveryDate);
    string NotificationChannel { get; }
}

public interface IProductionTracker
{
    void ScheduleProduction(List<Gift> gifts);
    int CalculateTotalBuildTime(List<Gift> gifts);
    void AssignElves(Gift gift);
}

// ========================================
// DISCOUNT STRATEGIES (OCP, SRP)
// ========================================

public class NiceListBonusDiscount : IGiftDiscountStrategy
{
    public string DiscountName => "Nice List Bonus (20% off)";

    public decimal CalculateDiscount(decimal totalValue, int niceScore)
    {
        if (niceScore >= 90)
            return totalValue * 0.20m;
        return 0m;
    }
}

public class SiblingBundleDiscount : IGiftDiscountStrategy
{
    public string DiscountName => "Sibling Bundle (Buy 2 Get 1 Free)";

    public decimal CalculateDiscount(decimal totalValue, int niceScore)
    {
        // Simplified: 33% off for bundle deals
        return totalValue * 0.33m;
    }
}

public class EarlyRequestDiscount : IGiftDiscountStrategy
{
    private readonly DateTime _cutoffDate;

    public EarlyRequestDiscount(DateTime? cutoffDate = null)
    {
        _cutoffDate = cutoffDate ?? new DateTime(DateTime.Now.Year, 12, 1);
    }

    public string DiscountName => "Early Bird Special (10% off)";

    public decimal CalculateDiscount(decimal totalValue, int niceScore)
    {
        if (DateTime.Now < _cutoffDate)
            return totalValue * 0.10m;
        return 0m;
    }
}

public class ReusedToyCreditDiscount : IGiftDiscountStrategy
{
    private readonly int _toysdonated;

    public ReusedToyCreditDiscount(int toysDonated)
    {
        _toysdonated = toysDonated;
    }

    public string DiscountName => "Toy Donation Credit";

    public decimal CalculateDiscount(decimal totalValue, int niceScore)
    {
        // $5 credit per toy donated
        decimal credit = _toysdonated * 5m;
        return Math.Min(credit, totalValue * 0.15m); // Max 15% off
    }
}

// ========================================
// WRAPPING SERVICES (OCP, SRP)
// ========================================

public class TraditionalPaperWrapping : IGiftWrappingService
{
    public decimal WrappingCost => 2.99m;
    public string WrappingType => "Traditional Red & Green Paper";

    public void WrapGift(Gift gift)
    {
        Console.WriteLine($"  🎁 Wrapping {gift.Name} in festive traditional paper");
    }
}

public class RecycledBrownPaperWrapping : IGiftWrappingService
{
    public decimal WrappingCost => 1.99m;
    public string WrappingType => "Eco-Friendly Recycled Paper";

    public void WrapGift(Gift gift)
    {
        Console.WriteLine($"  ♻️ Wrapping {gift.Name} in recycled brown paper with twine");
    }
}

public class MagicalInvisibleWrapping : IGiftWrappingService
{
    public decimal WrappingCost => 0m; // Free - it's magic!
    public string WrappingType => "Magical Invisible Wrap";

    public void WrapGift(Gift gift)
    {
        Console.WriteLine($"  ✨ Applying invisible magic wrap to {gift.Name}");
    }
}

public class GoldFoilDeluxeWrapping : IGiftWrappingService
{
    public decimal WrappingCost => 9.99m;
    public string WrappingType => "Deluxe Gold Foil";

    public void WrapGift(Gift gift)
    {
        Console.WriteLine($"  👑 Wrapping {gift.Name} in luxurious gold foil");
    }
}

// ========================================
// DELIVERY METHODS (OCP, SRP)
// ========================================

public class ClassicSleighDelivery : IDeliveryMethod
{
    public decimal DeliveryCost => 0m; // Free on Christmas!
    public string MethodName => "Classic 8-Reindeer Sleigh";

    public int CalculateDeliveryTime(string destination)
    {
        // 100 mph average
        return 60; // 1 hour for most deliveries
    }
}

public class TurboRudolphExpress : IDeliveryMethod
{
    public decimal DeliveryCost => 0m;
    public string MethodName => "Turbo Rudolph Express";

    public int CalculateDeliveryTime(string destination)
    {
        // Rudolph's red nose: twice as fast!
        return 30; // 30 minutes
    }
}

public class ChimneyTeleportation : IDeliveryMethod
{
    public decimal DeliveryCost => 5.99m; // Magic has a cost
    public string MethodName => "Instant Chimney Teleport";

    public int CalculateDeliveryTime(string destination)
    {
        return 1; // Instant!
    }
}

public class ElfDroneDelivery : IDeliveryMethod
{
    public decimal DeliveryCost => 3.99m;
    public string MethodName => "Autonomous Elf Drone";

    public int CalculateDeliveryTime(string destination)
    {
        return 45; // 45 minutes
    }
}

public class MagicTrainDelivery : IDeliveryMethod
{
    public decimal DeliveryCost => 0m;
    public string MethodName => "Polar Express Train";

    public int CalculateDeliveryTime(string destination)
    {
        return 40; // 40 minutes
    }
}

// ========================================
// NOTIFICATION SERVICES (OCP, SRP)
// ========================================

public class ChimneyLetterNotification : INotificationService
{
    public string NotificationChannel => "Chimney Letter";

    public void SendDeliveryNotification(GiftRequest request, DateTime deliveryDate)
    {
        Console.WriteLine($"  📬 Sending chimney letter to {request.ChildName}");
        Console.WriteLine($"     Delivery: {deliveryDate:MMMM dd, yyyy}");
    }
}

public class DreamMessageNotification : INotificationService
{
    public string NotificationChannel => "Dream Message";

    public void SendDeliveryNotification(GiftRequest request, DateTime deliveryDate)
    {
        Console.WriteLine($"  💭 Sending dream message to {request.ChildName}");
        Console.WriteLine($"     Will appear in dreams tonight!");
    }
}

public class ParentEmailNotification : INotificationService
{
    public string NotificationChannel => "Parent Email";

    public void SendDeliveryNotification(GiftRequest request, DateTime deliveryDate)
    {
        Console.WriteLine($"  📧 Sending email to parents of {request.ChildName}");
        Console.WriteLine($"     Tracking info included");
    }
}

public class CookieMessageNotification : INotificationService
{
    public string NotificationChannel => "Frosted Cookie Message";

    public void SendDeliveryNotification(GiftRequest request, DateTime deliveryDate)
    {
        Console.WriteLine($"  🍪 Baking cookie message for {request.ChildName}");
        Console.WriteLine($"     Message written in icing!");
    }
}

public class NorthPoleTextNotification : INotificationService
{
    public string NotificationChannel => "North Pole SMS";

    public void SendDeliveryNotification(GiftRequest request, DateTime deliveryDate)
    {
        Console.WriteLine($"  📱 Sending SMS to {request.ChildName}'s parents");
        Console.WriteLine($"     Text: Gifts arriving {deliveryDate:MM/dd}!");
    }
}

// ========================================
// PRODUCTION TRACKER (SRP)
// ========================================

public class WorkshopProductionTracker : IProductionTracker
{
    public void ScheduleProduction(List<Gift> gifts)
    {
        Console.WriteLine($"  🏭 Scheduling production for {gifts.Count} gifts");
        foreach (var gift in gifts)
        {
            Console.WriteLine($"     - {gift.Name} ({gift.BuildTime} elf-hours)");
        }
    }

    public int CalculateTotalBuildTime(List<Gift> gifts)
    {
        return gifts.Sum(g => g.BuildTime);
    }

    public void AssignElves(Gift gift)
    {
        int elvesNeeded = (int)Math.Ceiling(gift.BuildTime / 2.0);
        Console.WriteLine($"     Assigning {elvesNeeded} elves to {gift.Name}");
    }
}

// ========================================
// HIGH-LEVEL ORCHESTRATOR (DIP, SRP)
// ========================================

public class NorthPoleGiftProcessor
{
    private readonly IGiftDiscountStrategy _discountStrategy;
    private readonly IGiftWrappingService _wrappingService;
    private readonly IDeliveryMethod _deliveryMethod;
    private readonly INotificationService _notificationService;
    private readonly IProductionTracker _productionTracker;

    // Constructor Injection (DIP)
    public NorthPoleGiftProcessor(
        IGiftDiscountStrategy discountStrategy,
        IGiftWrappingService wrappingService,
        IDeliveryMethod deliveryMethod,
        INotificationService notificationService,
        IProductionTracker productionTracker)
    {
        _discountStrategy = discountStrategy ?? throw new ArgumentNullException(nameof(discountStrategy));
        _wrappingService = wrappingService ?? throw new ArgumentNullException(nameof(wrappingService));
        _deliveryMethod = deliveryMethod ?? throw new ArgumentNullException(nameof(deliveryMethod));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _productionTracker = productionTracker ?? throw new ArgumentNullException(nameof(productionTracker));
    }

    public void ProcessGiftRequest(GiftRequest request)
    {
        Console.WriteLine($"\n{'='}.PadRight(60, '=')}");
        Console.WriteLine($"🎅 Processing Request for {request.ChildName}");
        Console.WriteLine($"{'='}.PadRight(60, '=')}");

        // Step 1: Calculate costs
        decimal giftsCost = request.RequestedGifts.Count * 20m; // $20 per gift (simplified)
        decimal discount = _discountStrategy.CalculateDiscount(giftsCost, request.NiceScore);
        decimal wrappingCost = request.RequestedGifts.Count * _wrappingService.WrappingCost;
        decimal deliveryCost = _deliveryMethod.DeliveryCost;
        decimal totalCost = giftsCost - discount + wrappingCost + deliveryCost;

        Console.WriteLine($"\n💰 Cost Breakdown:");
        Console.WriteLine($"  Gifts: ${giftsCost:F2}");
        Console.WriteLine($"  Discount ({_discountStrategy.DiscountName}): -${discount:F2}");
        Console.WriteLine($"  Wrapping ({_wrappingService.WrappingType}): ${wrappingCost:F2}");
        Console.WriteLine($"  Delivery ({_deliveryMethod.MethodName}): ${deliveryCost:F2}");
        Console.WriteLine($"  TOTAL: ${totalCost:F2}");

        // Step 2: Schedule production
        Console.WriteLine($"\n🏭 Production:");
        _productionTracker.ScheduleProduction(request.RequestedGifts);
        int totalBuildTime = _productionTracker.CalculateTotalBuildTime(request.RequestedGifts);
        Console.WriteLine($"  Total build time: {totalBuildTime} elf-hours");

        foreach (var gift in request.RequestedGifts)
        {
            _productionTracker.AssignElves(gift);
        }

        // Step 3: Wrap gifts
        Console.WriteLine($"\n🎁 Gift Wrapping:");
        foreach (var gift in request.RequestedGifts)
        {
            _wrappingService.WrapGift(gift);
        }

        // Step 4: Arrange delivery
        Console.WriteLine($"\n🦌 Delivery:");
        int deliveryTime = _deliveryMethod.CalculateDeliveryTime(request.Address);
        DateTime deliveryDate = DateTime.Now.AddMinutes(deliveryTime);
        Console.WriteLine($"  Method: {_deliveryMethod.MethodName}");
        Console.WriteLine($"  Time: {deliveryTime} minutes");
        Console.WriteLine($"  Estimated arrival: {deliveryDate:MM/dd/yyyy HH:mm}");

        // Step 5: Send notification
        Console.WriteLine($"\n📢 Notification:");
        Console.WriteLine($"  Channel: {_notificationService.NotificationChannel}");
        _notificationService.SendDeliveryNotification(request, deliveryDate);

        Console.WriteLine($"\n✅ Request processed successfully!");
        Console.WriteLine($"{'='}.PadRight(60, '=')}");
    }
}

// ========================================
// DEMONSTRATION
// ========================================
public class CombinedSolutionDemo
{
    public static void Run()
    {
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine("SOLUTION: Combined SOLID Principles Challenge");
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine();

        // Create sample gift requests
        var giftRequest1 = new GiftRequest
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
            NiceScore = 95
        };

        var giftRequest2 = new GiftRequest
        {
            ChildName = "Oliver",
            Age = 6,
            Address = "456 Snowflake Street, North Pole",
            RequestedGifts = new List<Gift>
            {
                new Gift { Name = "Robot Kit", Category = "STEM", BuildTime = 4 },
                new Gift { Name = "Books", Category = "Educational", BuildTime = 1 }
            },
            NiceScore = 88
        };

        // Scenario 1: High nice score child with premium service
        Console.WriteLine("SCENARIO 1: Premium Service for Very Nice Child");
        Console.WriteLine("-" .PadRight(70, '-'));

        var processor1 = new NorthPoleGiftProcessor(
            new NiceListBonusDiscount(),
            new GoldFoilDeluxeWrapping(),
            new TurboRudolphExpress(),
            new DreamMessageNotification(),
            new WorkshopProductionTracker()
        );

        processor1.ProcessGiftRequest(giftRequest1);

        // Scenario 2: Eco-friendly delivery
        Console.WriteLine("\n\nSCENARIO 2: Eco-Friendly Budget Option");
        Console.WriteLine("-" .PadRight(70, '-'));

        var processor2 = new NorthPoleGiftProcessor(
            new SiblingBundleDiscount(),
            new RecycledBrownPaperWrapping(),
            new MagicTrainDelivery(),
            new ParentEmailNotification(),
            new WorkshopProductionTracker()
        );

        processor2.ProcessGiftRequest(giftRequest2);

        // Scenario 3: Express delivery
        Console.WriteLine("\n\nSCENARIO 3: Express Last-Minute Delivery");
        Console.WriteLine("-" .PadRight(70, '-'));

        var giftRequest3 = new GiftRequest
        {
            ChildName = "Sophia",
            Age = 10,
            Address = "789 Gingerbread Ave, Cookie Town",
            RequestedGifts = new List<Gift>
            {
                new Gift { Name = "Laptop", Category = "Electronics", BuildTime = 3 }
            },
            NiceScore = 92
        };

        var processor3 = new NorthPoleGiftProcessor(
            new EarlyRequestDiscount(),
            new MagicalInvisibleWrapping(),
            new ChimneyTeleportation(),
            new CookieMessageNotification(),
            new WorkshopProductionTracker()
        );

        processor3.ProcessGiftRequest(giftRequest3);

        // Show SOLID principles in action
        Console.WriteLine("\n\n" + "=" .PadRight(70, '='));
        Console.WriteLine("SOLID PRINCIPLES DEMONSTRATED:");
        Console.WriteLine("=" .PadRight(70, '='));

        Console.WriteLine("\n✓ SRP (Single Responsibility):");
        Console.WriteLine("  Each class has ONE job:");
        Console.WriteLine("  - Discount strategies calculate discounts");
        Console.WriteLine("  - Wrapping services wrap gifts");
        Console.WriteLine("  - Delivery methods handle delivery");
        Console.WriteLine("  - Notification services send notifications");
        Console.WriteLine("  - Production tracker manages workshop");
        Console.WriteLine("  - Processor orchestrates workflow");

        Console.WriteLine("\n✓ OCP (Open/Closed):");
        Console.WriteLine("  Easy to add new implementations without modifying existing code:");
        Console.WriteLine("  - Add new discount strategy");
        Console.WriteLine("  - Add new wrapping service");
        Console.WriteLine("  - Add new delivery method");
        Console.WriteLine("  - Add new notification channel");

        Console.WriteLine("\n✓ LSP (Liskov Substitution):");
        Console.WriteLine("  All implementations are substitutable:");
        Console.WriteLine("  - Any IGiftDiscountStrategy works");
        Console.WriteLine("  - Any IGiftWrappingService works");
        Console.WriteLine("  - Any IDeliveryMethod works");
        Console.WriteLine("  - No exceptions, all fulfill contracts");

        Console.WriteLine("\n✓ ISP (Interface Segregation):");
        Console.WriteLine("  Focused, single-purpose interfaces:");
        Console.WriteLine("  - IGiftDiscountStrategy (just discounts)");
        Console.WriteLine("  - IGiftWrappingService (just wrapping)");
        Console.WriteLine("  - IDeliveryMethod (just delivery)");
        Console.WriteLine("  - No fat interfaces forcing unused methods");

        Console.WriteLine("\n✓ DIP (Dependency Inversion):");
        Console.WriteLine("  High-level processor depends on abstractions:");
        Console.WriteLine("  - Constructor injection of all dependencies");
        Console.WriteLine("  - Easy to test with mocks");
        Console.WriteLine("  - No 'new' keyword for dependencies");
        Console.WriteLine("  - Loose coupling throughout");

        Console.WriteLine("\n" + "=" .PadRight(70, '='));
        Console.WriteLine();
    }
}
