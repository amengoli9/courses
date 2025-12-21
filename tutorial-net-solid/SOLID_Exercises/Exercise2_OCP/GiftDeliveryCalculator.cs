namespace Exercise2_OCP;

/// <summary>
/// Exercise 2: Open/Closed Principle (OCP)
/// Reindeer Delivery Method Problem
///
/// PROBLEM: Santa's gift delivery calculator needs modification every time
/// a new delivery method is invented. The elves are tired of changing tested code!
///
/// YOUR TASK:
/// 1. Refactor using the Strategy pattern so new delivery methods can be added
///    without modifying existing code
/// 2. Add delivery methods without touching the calculator
/// 3. Implement at least 5 different Christmas delivery strategies
///
/// PRINCIPLE: "Software entities should be open for extension, but closed for modification"
/// </summary>
public class GiftDeliveryCalculator
{
    public int CalculateDeliveryTime(int distance, string deliveryMethod)
    {
        if (deliveryMethod == "ClassicSleigh")
        {
            // Traditional sleigh: 100 miles per hour
            return distance / 100;
        }
        else if (deliveryMethod == "TurboReindeer")
        {
            // Rudolph's red nose gives extra speed: 200 mph
            return distance / 200;
        }
        else if (deliveryMethod == "MagicTeleport")
        {
            // Instant delivery via chimney magic
            return 1; // Always 1 minute
        }
        else if (deliveryMethod == "DroneElf")
        {
            // Modern elf drone: 150 mph
            return distance / 150;
        }

        return 999; // Unknown method takes forever!
    }
}

/*
 * ========================================
 * YOUR SOLUTION GOES BELOW THIS LINE
 * ========================================
 *
 * Create the following using Strategy Pattern:
 *
 * 1. Define IGiftDeliveryStrategy interface with:
 *    - int CalculateDeliveryTime(int distanceInMiles)
 *    - string DeliveryMethodName { get; }
 *    - string Description { get; }
 *
 * 2. Create concrete implementations:
 *    - ClassicSleighDelivery (Dasher, Dancer, Prancer, Vixen...)
 *    - TurboReindeerDelivery (Rudolph leading!)
 *    - MagicChimneyTeleport (instant via magic)
 *    - DroneElfDelivery (modern technology)
 *    - ExpressPolarBearDelivery (new method - add without modifying!)
 *
 * 3. Refactor GiftDeliveryCalculator to use the strategy
 *
 * Now when you add a new delivery method, you DON'T modify existing code!
 * This follows OCP - open for extension, closed for modification.
 */

// TODO: Create your IGiftDeliveryStrategy interface here

// TODO: Create concrete delivery strategy implementations

// TODO: Create a new GiftDeliveryCalculator that uses strategies
