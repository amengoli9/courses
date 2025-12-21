namespace Exercise4_ISP.Solution;

/// <summary>
/// SOLUTION: Interface Segregation Principle
/// Clients should not be forced to depend on interfaces they don't use.
///
/// Break the fat IWorkshopWorker interface into focused, role-specific interfaces.
/// </summary>

// ========================================
// SEGREGATED INTERFACES - Each role has its own interface
// ========================================

public interface IToyMaker
{
    void MakeToys();
    void PaintToys();
    void AssembleToys();
}

public interface IReindeerCaretaker
{
    void FeedReindeer();
    void GroomReindeer();
    void TrainReindeer();
}

public interface ICookieBaker
{
    void BakeCookies();
    void DecorateCookies();
}

public interface ISleighMechanic
{
    void RepairSleigh();
    void PolishSleigh();
}

public interface IGiftWrapper
{
    void WrapGifts();
}

public interface IListManager
{
    void UpdateNaughtyNiceList();
}

// ========================================
// CONCRETE IMPLEMENTATIONS - Only implement needed interfaces
// ========================================

public class ToyMakerElf : IToyMaker, IGiftWrapper
{
    private readonly string _name;

    public ToyMakerElf(string name)
    {
        _name = name;
    }

    // IToyMaker implementation
    public void MakeToys()
    {
        Console.WriteLine($"🧝 {_name}: Making wooden trains and dolls!");
    }

    public void PaintToys()
    {
        Console.WriteLine($"🎨 {_name}: Painting toys red and green!");
    }

    public void AssembleToys()
    {
        Console.WriteLine($"🔧 {_name}: Assembling toy parts with precision!");
    }

    // IGiftWrapper implementation
    public void WrapGifts()
    {
        Console.WriteLine($"🎁 {_name}: Wrapping gifts in festive paper!");
    }
}

public class ReindeerCaretaker : IReindeerCaretaker, ISleighMechanic
{
    private readonly string _name;

    public ReindeerCaretaker(string name)
    {
        _name = name;
    }

    // IReindeerCaretaker implementation
    public void FeedReindeer()
    {
        Console.WriteLine($"🦌 {_name}: Feeding carrots to the reindeer!");
    }

    public void GroomReindeer()
    {
        Console.WriteLine($"✨ {_name}: Brushing Rudolph's coat until it shines!");
    }

    public void TrainReindeer()
    {
        Console.WriteLine($"🎯 {_name}: Training reindeer for flight patterns!");
    }

    // ISleighMechanic implementation
    public void RepairSleigh()
    {
        Console.WriteLine($"🔨 {_name}: Repairing sleigh runners!");
    }

    public void PolishSleigh()
    {
        Console.WriteLine($"✨ {_name}: Polishing sleigh until it gleams!");
    }
}

public class MrsClaus : ICookieBaker
{
    // ICookieBaker implementation
    public void BakeCookies()
    {
        Console.WriteLine($"🍪 Mrs. Claus: Baking delicious gingerbread cookies!");
    }

    public void DecorateCookies()
    {
        Console.WriteLine($"🎨 Mrs. Claus: Decorating cookies with icing and sprinkles!");
    }
}

public class HeadElf : IToyMaker, IListManager, IGiftWrapper
{
    private readonly string _name;

    public HeadElf(string name)
    {
        _name = name;
    }

    // IToyMaker implementation
    public void MakeToys()
    {
        Console.WriteLine($"🧝‍♂️ {_name} (Head Elf): Overseeing toy production!");
    }

    public void PaintToys()
    {
        Console.WriteLine($"🎨 {_name} (Head Elf): Quality checking paint jobs!");
    }

    public void AssembleToys()
    {
        Console.WriteLine($"🔧 {_name} (Head Elf): Supervising toy assembly!");
    }

    // IListManager implementation
    public void UpdateNaughtyNiceList()
    {
        Console.WriteLine($"📋 {_name} (Head Elf): Updating the Naughty & Nice list!");
    }

    // IGiftWrapper implementation
    public void WrapGifts()
    {
        Console.WriteLine($"🎁 {_name} (Head Elf): Wrapping special VIP gifts!");
    }
}

public class SantaClaus : IListManager
{
    // IListManager implementation
    public void UpdateNaughtyNiceList()
    {
        Console.WriteLine($"🎅 Santa: Checking the list twice!");
    }

    // Santa's special methods (not from interfaces)
    public void DeliverGifts()
    {
        Console.WriteLine($"🎅 Santa: Ho Ho Ho! Delivering gifts around the world!");
    }

    public void TasteCookies()
    {
        Console.WriteLine($"🍪 Santa: Quality testing Mrs. Claus's cookies!");
    }

    public void GiveTreatsToReindeer()
    {
        Console.WriteLine($"🦌 Santa: Giving special treats to Rudolph and friends!");
    }
}

public class SleighMechanic : ISleighMechanic
{
    private readonly string _name;

    public SleighMechanic(string name)
    {
        _name = name;
    }

    public void RepairSleigh()
    {
        Console.WriteLine($"🔨 {_name}: Repairing and upgrading the sleigh!");
    }

    public void PolishSleigh()
    {
        Console.WriteLine($"✨ {_name}: Polishing the sleigh to perfection!");
    }
}

public class GiftWrapSpecialist : IGiftWrapper
{
    private readonly string _name;

    public GiftWrapSpecialist(string name)
    {
        _name = name;
    }

    public void WrapGifts()
    {
        Console.WriteLine($"🎁 {_name}: Expert gift wrapping with artistic flair!");
    }
}

public class CookieBakerElf : ICookieBaker
{
    private readonly string _name;

    public CookieBakerElf(string name)
    {
        _name = name;
    }

    public void BakeCookies()
    {
        Console.WriteLine($"🍪 {_name}: Baking cookies for Santa and the elves!");
    }

    public void DecorateCookies()
    {
        Console.WriteLine($"🎨 {_name}: Creating beautiful cookie decorations!");
    }
}

// ========================================
// WORKSHOP MANAGEMENT - Works with focused interfaces
// ========================================
public class WorkshopManager
{
    public void OrganizeToyProduction(List<IToyMaker> toyMakers)
    {
        Console.WriteLine("\n🏭 ORGANIZING TOY PRODUCTION:");
        foreach (var maker in toyMakers)
        {
            maker.MakeToys();
            maker.PaintToys();
            maker.AssembleToys();
        }
    }

    public void ManageReindeerCare(List<IReindeerCaretaker> caretakers)
    {
        Console.WriteLine("\n🦌 MANAGING REINDEER CARE:");
        foreach (var caretaker in caretakers)
        {
            caretaker.FeedReindeer();
            caretaker.GroomReindeer();
            caretaker.TrainReindeer();
        }
    }

    public void PrepareCookies(List<ICookieBaker> bakers)
    {
        Console.WriteLine("\n🍪 PREPARING COOKIES:");
        foreach (var baker in bakers)
        {
            baker.BakeCookies();
            baker.DecorateCookies();
        }
    }

    public void MaintainSleigh(List<ISleighMechanic> mechanics)
    {
        Console.WriteLine("\n🛷 MAINTAINING SLEIGH:");
        foreach (var mechanic in mechanics)
        {
            mechanic.RepairSleigh();
            mechanic.PolishSleigh();
        }
    }

    public void WrapAllGifts(List<IGiftWrapper> wrappers)
    {
        Console.WriteLine("\n🎁 WRAPPING GIFTS:");
        foreach (var wrapper in wrappers)
        {
            wrapper.WrapGifts();
        }
    }

    public void UpdateList(List<IListManager> managers)
    {
        Console.WriteLine("\n📋 UPDATING NAUGHTY & NICE LIST:");
        foreach (var manager in managers)
        {
            manager.UpdateNaughtyNiceList();
        }
    }
}

// ========================================
// DEMONSTRATION
// ========================================
public class IspSolutionDemo
{
    public static void Run()
    {
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("SOLUTION: Interface Segregation Principle");
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine();

        // Create all the workshop workers
        var toyMakerElf1 = new ToyMakerElf("Jingles");
        var toyMakerElf2 = new ToyMakerElf("Sparkle");
        var headElf = new HeadElf("Bernard");

        var reindeerCaretaker = new ReindeerCaretaker("Holly");
        var sleighMechanic = new SleighMechanic("Tinker");

        var mrsClaus = new MrsClaus();
        var cookieBaker = new CookieBakerElf("Sugar");

        var giftWrapper = new GiftWrapSpecialist("Ribbon");

        var santa = new SantaClaus();

        // Use the workshop manager
        var manager = new WorkshopManager();

        // Each worker is used ONLY for what they can do
        manager.OrganizeToyProduction(new List<IToyMaker>
        {
            toyMakerElf1,
            toyMakerElf2,
            headElf
        });

        manager.ManageReindeerCare(new List<IReindeerCaretaker>
        {
            reindeerCaretaker
        });

        manager.PrepareCookies(new List<ICookieBaker>
        {
            mrsClaus,
            cookieBaker
        });

        manager.MaintainSleigh(new List<ISleighMechanic>
        {
            reindeerCaretaker,  // Can also maintain sleigh!
            sleighMechanic
        });

        manager.WrapAllGifts(new List<IGiftWrapper>
        {
            toyMakerElf1,  // Toy makers can also wrap!
            toyMakerElf2,
            headElf,
            giftWrapper
        });

        manager.UpdateList(new List<IListManager>
        {
            headElf,
            santa
        });

        // Santa's special activities
        Console.WriteLine("\n🎅 SANTA'S SPECIAL DUTIES:");
        santa.DeliverGifts();
        santa.TasteCookies();
        santa.GiveTreatsToReindeer();

        Console.WriteLine();
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("BENEFITS OF ISP:");
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("✓ Workers only implement interfaces they actually use");
        Console.WriteLine("✓ No NotSupportedException - all methods are meaningful");
        Console.WriteLine("✓ Each interface is focused and cohesive");
        Console.WriteLine("✓ Easy to add new worker types with different combinations");
        Console.WriteLine("✓ Clients depend only on the methods they need");
        Console.WriteLine("✓ Better flexibility - workers can implement multiple roles");
        Console.WriteLine();

        Console.WriteLine("KEY INSIGHT:");
        Console.WriteLine("  Fat interfaces force unnecessary dependencies.");
        Console.WriteLine("  Segregated interfaces give clients exactly what they need.");
        Console.WriteLine("  Workers implement only what makes sense for their role!");
        Console.WriteLine();
    }
}
