namespace Exercise3_LSP;

/// <summary>
/// Exercise 3: Liskov Substitution Principle (LSP)
/// Gift Wrapper Hierarchy Problem
///
/// PROBLEM: The North Pole gift wrapping system has a broken class hierarchy
/// that violates LSP. Some wrappers throw NotSupportedException!
///
/// YOUR TASK:
/// 1. Redesign the class hierarchy to follow LSP
/// 2. Ensure all gift wrappers can be used interchangeably where expected
/// 3. Make decorations optional, not mandatory
///
/// PRINCIPLE: "Objects of a superclass should be replaceable with objects of a subclass
/// without breaking the application"
/// </summary>
public abstract class GiftWrapper
{
    public abstract void WrapGift(string giftName);
    public abstract void AddRibbon();
    public abstract void AddBow();
}

public class StandardGiftWrapper : GiftWrapper
{
    public override void WrapGift(string giftName)
    {
        Console.WriteLine($"Wrapping {giftName} in festive paper");
    }

    public override void AddRibbon()
    {
        Console.WriteLine("Adding beautiful ribbon");
    }

    public override void AddBow()
    {
        Console.WriteLine("Placing a bow on top");
    }
}

public class EdibleGiftWrapper : GiftWrapper
{
    // Wraps gifts in edible candy cane wrapping
    public override void WrapGift(string giftName)
    {
        Console.WriteLine($"Wrapping {giftName} in candy cane wrapper");
    }

    public override void AddRibbon()
    {
        // Edible gifts can't have ribbon - it's not food safe!
        throw new NotSupportedException("Edible gifts cannot have non-edible ribbon!");
    }

    public override void AddBow()
    {
        // Can make edible chocolate bow
        Console.WriteLine("Adding chocolate bow");
    }
}

public class InvisibleGiftWrapper : GiftWrapper
{
    // For surprise gifts that shouldn't be wrapped
    public override void WrapGift(string giftName)
    {
        // Invisible wrapping - do nothing!
        Console.WriteLine("Gift remains unwrapped (invisible wrap)");
    }

    public override void AddRibbon()
    {
        throw new NotSupportedException("Invisible wrapping doesn't support ribbons!");
    }

    public override void AddBow()
    {
        throw new NotSupportedException("Invisible wrapping doesn't support bows!");
    }
}

public class ElfWorkshop
{
    public void PrepareGift(GiftWrapper wrapper, string gift)
    {
        wrapper.WrapGift(gift);
        wrapper.AddRibbon(); // This throws exception for some wrappers!
        wrapper.AddBow();    // This also throws exceptions!
    }
}

/*
 * ========================================
 * YOUR SOLUTION GOES BELOW THIS LINE
 * ========================================
 *
 * Redesign using Interface Segregation:
 *
 * 1. Create focused interfaces:
 *    - IGiftWrapper (just wrapping)
 *    - IRibbonDecorator (optional ribbon capability)
 *    - IBowDecorator (optional bow capability)
 *
 * 2. Create implementations that implement ONLY what they support:
 *    - StandardGiftWrapper : IGiftWrapper, IRibbonDecorator, IBowDecorator
 *    - EdibleGiftWrapper : IGiftWrapper, IBowDecorator (only edible bow!)
 *    - InvisibleGiftWrapper : IGiftWrapper (no decorations)
 *
 * 3. Update ElfWorkshop to check capabilities before decorating
 *
 * Now all wrappers are substitutable - no exceptions thrown!
 * This follows LSP - subtypes can replace base types without errors.
 */

// TODO: Create your interfaces here

// TODO: Create improved wrapper implementations

// TODO: Create improved ElfWorkshop that respects LSP
