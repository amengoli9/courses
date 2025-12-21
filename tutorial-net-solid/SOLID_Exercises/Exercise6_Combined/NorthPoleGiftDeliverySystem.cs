namespace Exercise6_Combined;

/// <summary>
/// Exercise 6: Combined Challenge - North Pole Gift Delivery System
/// The Ultimate Christmas Workshop Challenge
///
/// Build Santa's complete gift delivery system that follows ALL SOLID principles.
///
/// REQUIREMENTS:
/// 1. Multiple Gift Wrapping Methods (Traditional, Recycled, Invisible, Gold Foil)
/// 2. Different Discount Strategies (Nice List Bonus, Sibling Bundle, Early Request, Reused Toy Credit)
/// 3. Multiple Delivery Methods (Classic Sleigh, Turbo Rudolph, Chimney Teleport, Elf Drone, Magic Train)
/// 4. Various Notification Channels (Chimney Letter, Dream Message, Parent Email, Cookie Message, Text)
/// 5. Workshop Production Tracking (Build time, Elf assignments, Capacity, Reports)
///
/// YOUR SYSTEM MUST DEMONSTRATE ALL SOLID PRINCIPLES:
/// ✅ SRP: Each class has a single, clear responsibility
/// ✅ OCP: Can add new strategies without modifying existing code
/// ✅ LSP: All implementations are substitutable
/// ✅ ISP: Small, focused interfaces
/// ✅ DIP: High-level logic doesn't depend on concrete implementations
/// </summary>

// Core data models
public class Gift
{
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public int BuildTime { get; set; } // in elf-hours
}

public class GiftRequest
{
    public string ChildName { get; set; } = "";
    public int Age { get; set; }
    public string Address { get; set; } = "";
    public List<Gift> RequestedGifts { get; set; } = new();
    public int NiceScore { get; set; } // 0-100
}

/*
 * ========================================
 * YOUR SOLUTION GOES BELOW THIS LINE
 * ========================================
 *
 * Design and implement the complete system following ALL SOLID principles.
 *
 * STEP 1: Define your core abstractions (interfaces)
 * --------------------------------------------------
 */

// TODO: Create IGiftDiscountStrategy interface
// Example:
// public interface IGiftDiscountStrategy
// {
//     decimal CalculateDiscount(decimal totalValue, int niceScore);
//     string DiscountName { get; }
// }

// TODO: Create IGiftWrappingService interface
// Example:
// public interface IGiftWrappingService
// {
//     void WrapGift(Gift gift);
//     decimal WrappingCost { get; }
// }

// TODO: Create IDeliveryMethod interface
// Example:
// public interface IDeliveryMethod
// {
//     int CalculateDeliveryTime(string destination);
//     decimal DeliveryCost { get; }
//     string MethodName { get; }
// }

// TODO: Create INotificationService interface
// Example:
// public interface INotificationService
// {
//     void SendDeliveryNotification(GiftRequest request, DateTime deliveryDate);
// }

// TODO: Create IProductionTracker interface
// Example:
// public interface IProductionTracker
// {
//     void ScheduleProduction(List<Gift> gifts);
//     int CalculateTotalBuildTime(List<Gift> gifts);
//     void AssignElves(Gift gift);
// }

/*
 * STEP 2: Implement concrete strategies (at least 3 for each interface)
 * ----------------------------------------------------------------------
 */

// TODO: Implement discount strategies:
// - NiceListBonusDiscount (20% for nice score > 90)
// - SiblingBundleDiscount (buy 2 get 1 free)
// - EarlyRequestDiscount (10% for requests before Dec 1)
// - ReusedToyCreditDiscount (discount for donations)

// TODO: Implement wrapping services:
// - TraditionalPaperWrapping
// - RecycledBrownPaperWrapping
// - MagicalInvisibleWrapping
// - GoldFoilDeluxeWrapping

// TODO: Implement delivery methods:
// - ClassicSleighDelivery
// - TurboRudolphExpress
// - ChimneyTeleportation
// - ElfDroneDelivery
// - MagicTrainDelivery

// TODO: Implement notification services:
// - ChimneyLetterNotification
// - DreamMessageNotification
// - ParentEmailNotification
// - CookieMessageNotification
// - NorthPoleTextNotification

// TODO: Implement production tracker:
// - WorkshopProductionTracker

/*
 * STEP 3: Create the high-level orchestrator (follows DIP!)
 * ----------------------------------------------------------
 */

// TODO: Implement NorthPoleGiftProcessor
// Example:
// public class NorthPoleGiftProcessor
// {
//     private readonly IGiftDiscountStrategy _discountStrategy;
//     private readonly IGiftWrappingService _wrappingService;
//     private readonly IDeliveryMethod _deliveryMethod;
//     private readonly INotificationService _notificationService;
//     private readonly IProductionTracker _productionTracker;
//
//     public NorthPoleGiftProcessor(
//         IGiftDiscountStrategy discountStrategy,
//         IGiftWrappingService wrappingService,
//         IDeliveryMethod deliveryMethod,
//         INotificationService notificationService,
//         IProductionTracker productionTracker)
//     {
//         _discountStrategy = discountStrategy;
//         _wrappingService = wrappingService;
//         _deliveryMethod = deliveryMethod;
//         _notificationService = notificationService;
//         _productionTracker = productionTracker;
//     }
//
//     public void ProcessGiftRequest(GiftRequest request)
//     {
//         // Your implementation here
//         // Should use all injected dependencies
//     }
// }

/*
 * EVALUATION CRITERIA:
 * ====================
 * ✅ SRP: Each class has exactly one responsibility
 * ✅ OCP: Can add new strategies without modifying existing code
 * ✅ LSP: All strategy implementations are truly interchangeable
 * ✅ ISP: Interfaces are small and focused
 * ✅ DIP: NorthPoleGiftProcessor depends only on abstractions
 * ✅ Testability: Easy to unit test with mocked dependencies
 * ✅ Extensibility: Simple to add new features
 * ✅ Code Quality: Clean, readable, well-organized
 *
 * BONUS CHALLENGES:
 * =================
 * 1. Add a Nice/Naughty Validator
 * 2. Implement a GiftRecommendationEngine
 * 3. Create a WorkshopCapacityManager
 * 4. Add a ChristmasEveCountdown notification
 * 5. Implement a CoalDeliveryService (following LSP!)
 */
