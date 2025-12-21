namespace Exercise2_OCP.Solution;

/// <summary>
/// SOLUTION: Open/Closed Principle
/// Open for extension, closed for modification.
/// New delivery methods can be added without modifying existing code.
/// </summary>

// ========================================
// ABSTRACTION - Define the contract
// ========================================
public interface IGiftDeliveryStrategy
{
    int CalculateDeliveryTime(int distanceInMiles);
    string DeliveryMethodName { get; }
    string Description { get; }
}

// ========================================
// CONCRETE STRATEGIES - Each delivery method
// ========================================

public class ClassicSleighDelivery : IGiftDeliveryStrategy
{
    public string DeliveryMethodName => "Classic Sleigh";
    public string Description => "Traditional 8-reindeer sleigh (Dasher, Dancer, Prancer, Vixen, Comet, Cupid, Donner, Blitzen)";

    public int CalculateDeliveryTime(int distanceInMiles)
    {
        // Classic sleigh: 100 miles per hour
        return distanceInMiles / 100;
    }
}

public class TurboReindeerDelivery : IGiftDeliveryStrategy
{
    public string DeliveryMethodName => "Turbo Rudolph Express";
    public string Description => "Rudolph's red nose provides turbo boost - twice as fast!";

    public int CalculateDeliveryTime(int distanceInMiles)
    {
        // Rudolph's red nose gives extra speed: 200 mph
        return distanceInMiles / 200;
    }
}

public class MagicChimneyTeleport : IGiftDeliveryStrategy
{
    public string DeliveryMethodName => "Magic Chimney Teleport";
    public string Description => "Instant delivery via chimney magic network";

    public int CalculateDeliveryTime(int distanceInMiles)
    {
        // Instant delivery via chimney magic - distance doesn't matter
        return 1; // Always 1 minute
    }
}

public class DroneElfDelivery : IGiftDeliveryStrategy
{
    public string DeliveryMethodName => "Elf Drone Delivery";
    public string Description => "Modern autonomous elf drones with GPS";

    public int CalculateDeliveryTime(int distanceInMiles)
    {
        // Modern elf drone: 150 mph
        return distanceInMiles / 150;
    }
}

public class ExpressPolarBearDelivery : IGiftDeliveryStrategy
{
    public string DeliveryMethodName => "Express Polar Bear";
    public string Description => "Specially trained polar bears for Arctic regions";

    public int CalculateDeliveryTime(int distanceInMiles)
    {
        // Polar bears: 75 mph (slower but good in snow)
        return distanceInMiles / 75;
    }
}

public class MagicTrainDelivery : IGiftDeliveryStrategy
{
    public string DeliveryMethodName => "Polar Express Train";
    public string Description => "The magical Polar Express train";

    public int CalculateDeliveryTime(int distanceInMiles)
    {
        // Magic train: 250 mph on magical tracks
        return distanceInMiles / 250;
    }
}

public class RocketSleighDelivery : IGiftDeliveryStrategy
{
    public string DeliveryMethodName => "Rocket-Powered Sleigh";
    public string Description => "Next-gen rocket sleigh for distant deliveries";

    public int CalculateDeliveryTime(int distanceInMiles)
    {
        // Rocket sleigh: 500 mph!
        return distanceInMiles / 500;
    }
}

// ========================================
// CALCULATOR - Uses strategies (CLOSED for modification)
// ========================================
public class ImprovedGiftDeliveryCalculator
{
    private readonly IGiftDeliveryStrategy _strategy;

    public ImprovedGiftDeliveryCalculator(IGiftDeliveryStrategy strategy)
    {
        _strategy = strategy;
    }

    public int CalculateDeliveryTime(int distanceInMiles)
    {
        return _strategy.CalculateDeliveryTime(distanceInMiles);
    }

    public void DisplayDeliveryInfo(int distanceInMiles)
    {
        var time = CalculateDeliveryTime(distanceInMiles);
        Console.WriteLine($"🦌 Method: {_strategy.DeliveryMethodName}");
        Console.WriteLine($"   {_strategy.Description}");
        Console.WriteLine($"   Distance: {distanceInMiles} miles");
        Console.WriteLine($"   Estimated Time: {time} minutes");
    }
}

// ========================================
// DELIVERY SERVICE - Factory pattern (optional)
// ========================================
public class DeliveryStrategyFactory
{
    private readonly Dictionary<string, IGiftDeliveryStrategy> _strategies;

    public DeliveryStrategyFactory()
    {
        _strategies = new Dictionary<string, IGiftDeliveryStrategy>
        {
            { "classic", new ClassicSleighDelivery() },
            { "turbo", new TurboReindeerDelivery() },
            { "teleport", new MagicChimneyTeleport() },
            { "drone", new DroneElfDelivery() },
            { "polar-bear", new ExpressPolarBearDelivery() },
            { "train", new MagicTrainDelivery() },
            { "rocket", new RocketSleighDelivery() }
        };
    }

    public IGiftDeliveryStrategy GetStrategy(string key)
    {
        if (_strategies.TryGetValue(key, out var strategy))
        {
            return strategy;
        }

        // Default to classic sleigh
        return new ClassicSleighDelivery();
    }

    public IEnumerable<IGiftDeliveryStrategy> GetAllStrategies()
    {
        return _strategies.Values;
    }
}

// ========================================
// DEMONSTRATION
// ========================================
public class OcpSolutionDemo
{
    public static void Run()
    {
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("SOLUTION: Open/Closed Principle");
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine();

        int testDistance = 1000; // miles

        Console.WriteLine($"Testing all delivery methods for {testDistance} miles:\n");

        // Test each strategy
        var strategies = new List<IGiftDeliveryStrategy>
        {
            new ClassicSleighDelivery(),
            new TurboReindeerDelivery(),
            new MagicChimneyTeleport(),
            new DroneElfDelivery(),
            new ExpressPolarBearDelivery(),
            new MagicTrainDelivery(),
            new RocketSleighDelivery()
        };

        foreach (var strategy in strategies)
        {
            var calculator = new ImprovedGiftDeliveryCalculator(strategy);
            calculator.DisplayDeliveryInfo(testDistance);
            Console.WriteLine();
        }

        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("BENEFITS OF OCP:");
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("✓ New delivery methods can be added without modifying calculator");
        Console.WriteLine("✓ Existing code is CLOSED for modification");
        Console.WriteLine("✓ System is OPEN for extension via new strategy classes");
        Console.WriteLine("✓ Each strategy is independently testable");
        Console.WriteLine("✓ No if-else chains - polymorphism handles variation");
        Console.WriteLine();

        Console.WriteLine("ADDING A NEW STRATEGY:");
        Console.WriteLine("  1. Create new class implementing IGiftDeliveryStrategy");
        Console.WriteLine("  2. No need to modify ImprovedGiftDeliveryCalculator");
        Console.WriteLine("  3. No need to modify other strategies");
        Console.WriteLine("  4. Just inject the new strategy!");
        Console.WriteLine();
    }
}
