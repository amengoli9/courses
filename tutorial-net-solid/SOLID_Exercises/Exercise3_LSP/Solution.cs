namespace Exercise3_LSP.Solution;

/// <summary>
/// SOLUTION: Liskov Substitution Principle
/// Objects of a superclass should be replaceable with objects of a subclass
/// without breaking the application.
///
/// Key: Use interface segregation to avoid forcing implementations that don't apply.
/// </summary>

// ========================================
// SEGREGATED INTERFACES - Each capability is optional
// ========================================

public interface IGiftWrapper
{
    void WrapGift(string giftName);
    string WrapperType { get; }
}

public interface IRibbonDecorator
{
    void AddRibbon();
}

public interface IBowDecorator
{
    void AddBow();
}

// ========================================
// CONCRETE IMPLEMENTATIONS - Implement only what they support
// ========================================

public class StandardGiftWrapper : IGiftWrapper, IRibbonDecorator, IBowDecorator
{
    public string WrapperType => "Festive Paper Wrapper";

    public void WrapGift(string giftName)
    {
        Console.WriteLine($"🎁 Wrapping {giftName} in festive red and green paper");
    }

    public void AddRibbon()
    {
        Console.WriteLine($"🎀 Adding beautiful silk ribbon");
    }

    public void AddBow()
    {
        Console.WriteLine($"🎀 Placing a decorative bow on top");
    }
}

public class EdibleGiftWrapper : IGiftWrapper, IBowDecorator
{
    public string WrapperType => "Candy Cane Wrapper";

    public void WrapGift(string giftName)
    {
        Console.WriteLine($"🍬 Wrapping {giftName} in edible candy cane wrapper");
    }

    public void AddBow()
    {
        // Can add edible chocolate bow
        Console.WriteLine($"🍫 Adding chocolate bow (food-safe decoration)");
    }

    // Note: Does NOT implement IRibbonDecorator because edible gifts
    // can't have non-edible ribbon!
}

public class InvisibleGiftWrapper : IGiftWrapper
{
    public string WrapperType => "Invisible Surprise Wrap";

    public void WrapGift(string giftName)
    {
        // Invisible wrapping - the gift remains unwrapped for surprise!
        Console.WriteLine($"✨ Gift {giftName} remains unwrapped (invisible magic wrap)");
    }

    // Note: Does NOT implement IRibbonDecorator or IBowDecorator
    // because invisible wrapping doesn't support decorations
}

public class RecycledPaperWrapper : IGiftWrapper, IRibbonDecorator
{
    public string WrapperType => "Eco-Friendly Recycled Wrapper";

    public void WrapGift(string giftName)
    {
        Console.WriteLine($"♻️ Wrapping {giftName} in eco-friendly recycled paper");
    }

    public void AddRibbon()
    {
        Console.WriteLine($"🌿 Adding natural twine ribbon");
    }

    // Note: Has ribbon but no bow (minimalist design)
}

public class GoldFoilWrapper : IGiftWrapper, IRibbonDecorator, IBowDecorator
{
    public string WrapperType => "Deluxe Gold Foil Wrapper";

    public void WrapGift(string giftName)
    {
        Console.WriteLine($"✨ Wrapping {giftName} in luxurious gold foil");
    }

    public void AddRibbon()
    {
        Console.WriteLine($"👑 Adding premium gold ribbon");
    }

    public void AddBow()
    {
        Console.WriteLine($"👑 Placing elegant gold bow");
    }
}

// ========================================
// IMPROVED WORKSHOP - Respects LSP
// ========================================
public class ImprovedElfWorkshop
{
    public void PrepareGift(IGiftWrapper wrapper, string gift)
    {
        Console.WriteLine($"\n🧝 Preparing gift: {gift}");
        Console.WriteLine($"   Using: {wrapper.WrapperType}");
        Console.WriteLine();

        // Always wrap the gift (all wrappers support this)
        wrapper.WrapGift(gift);

        // Only add ribbon if the wrapper supports it
        if (wrapper is IRibbonDecorator ribbonDecorator)
        {
            ribbonDecorator.AddRibbon();
        }
        else
        {
            Console.WriteLine($"   (No ribbon for this wrapper type)");
        }

        // Only add bow if the wrapper supports it
        if (wrapper is IBowDecorator bowDecorator)
        {
            bowDecorator.AddBow();
        }
        else
        {
            Console.WriteLine($"   (No bow for this wrapper type)");
        }

        Console.WriteLine($"✅ Gift {gift} is ready!\n");
    }

    public void PrepareGiftBatch(List<(IGiftWrapper wrapper, string gift)> gifts)
    {
        foreach (var (wrapper, gift) in gifts)
        {
            PrepareGift(wrapper, gift);
        }
    }
}

// ========================================
// ALTERNATIVE: Using explicit decoration methods
// ========================================
public class GiftPreparationService
{
    public void WrapGift(IGiftWrapper wrapper, string giftName)
    {
        wrapper.WrapGift(giftName);
    }

    public void AddRibbon(IGiftWrapper wrapper)
    {
        if (wrapper is IRibbonDecorator ribbonDecorator)
        {
            ribbonDecorator.AddRibbon();
        }
        else
        {
            throw new InvalidOperationException(
                $"{wrapper.WrapperType} does not support ribbon decoration");
        }
    }

    public void AddBow(IGiftWrapper wrapper)
    {
        if (wrapper is IBowDecorator bowDecorator)
        {
            bowDecorator.AddBow();
        }
        else
        {
            throw new InvalidOperationException(
                $"{wrapper.WrapperType} does not support bow decoration");
        }
    }

    public bool SupportsRibbon(IGiftWrapper wrapper)
    {
        return wrapper is IRibbonDecorator;
    }

    public bool SupportsBow(IGiftWrapper wrapper)
    {
        return wrapper is IBowDecorator;
    }
}

// ========================================
// DEMONSTRATION
// ========================================
public class LspSolutionDemo
{
    public static void Run()
    {
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("SOLUTION: Liskov Substitution Principle");
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine();

        var workshop = new ImprovedElfWorkshop();

        // Create different wrapper types
        var wrappers = new List<(IGiftWrapper wrapper, string gift)>
        {
            (new StandardGiftWrapper(), "Teddy Bear"),
            (new EdibleGiftWrapper(), "Chocolate Santa"),
            (new InvisibleGiftWrapper(), "Surprise Gift"),
            (new RecycledPaperWrapper(), "Wooden Toy"),
            (new GoldFoilWrapper(), "Diamond Ring")
        };

        // All wrappers can be used interchangeably!
        workshop.PrepareGiftBatch(wrappers);

        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("BENEFITS OF LSP:");
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("✓ All IGiftWrapper implementations are truly substitutable");
        Console.WriteLine("✓ No NotSupportedException thrown");
        Console.WriteLine("✓ Decorations are optional capabilities, not requirements");
        Console.WriteLine("✓ Workshop code doesn't break with any wrapper type");
        Console.WriteLine("✓ Each wrapper implements only what it can actually do");
        Console.WriteLine();

        Console.WriteLine("KEY INSIGHT:");
        Console.WriteLine("  LSP + ISP work together!");
        Console.WriteLine("  - Segregated interfaces (ISP) enable proper substitution (LSP)");
        Console.WriteLine("  - Classes only implement what they can fulfill");
        Console.WriteLine("  - Clients check capabilities before using optional features");
        Console.WriteLine();

        // Demonstrate capability checking
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("CAPABILITY CHECKING:");
        Console.WriteLine("=" .PadRight(60, '='));

        var service = new GiftPreparationService();
        var testWrapper = new EdibleGiftWrapper();

        Console.WriteLine($"\nTesting {testWrapper.WrapperType}:");
        Console.WriteLine($"  Supports Ribbon? {service.SupportsRibbon(testWrapper)}");
        Console.WriteLine($"  Supports Bow? {service.SupportsBow(testWrapper)}");
        Console.WriteLine();
    }
}
