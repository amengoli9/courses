namespace Exercise4_ISP;

/// <summary>
/// Exercise 4: Interface Segregation Principle (ISP)
/// North Pole Workshop Worker Problem
///
/// PROBLEM: The "fat" interface forces all workshop workers to implement
/// abilities they don't have, leading to NotSupportedException everywhere!
///
/// YOUR TASK:
/// 1. Break the fat interface into smaller, role-specific interfaces
/// 2. Have each workshop worker implement only the interfaces they need
/// 3. Create implementations for at least 5 different North Pole workers
///
/// PRINCIPLE: "Clients should not be forced to depend on interfaces they don't use"
/// </summary>
public interface IWorkshopWorker
{
    // Toy making abilities
    void MakeToys();
    void PaintToys();
    void AssembleToys();

    // Reindeer care abilities
    void FeedReindeer();
    void GroomReindeer();
    void TrainReindeer();

    // Cookie baking abilities
    void BakeCookies();
    void DecorateCookies();

    // Sleigh maintenance
    void RepairSleigh();
    void PolishSleigh();

    // Gift wrapping
    void WrapGifts();

    // List management
    void UpdateNaughtyNiceList();
}

public class ToyMakerElf : IWorkshopWorker
{
    // Toy making - these are OK!
    public void MakeToys() { Console.WriteLine("Making wooden trains!"); }
    public void PaintToys() { Console.WriteLine("Painting toys red and green!"); }
    public void AssembleToys() { Console.WriteLine("Assembling toy parts!"); }

    // But forced to implement all these even though elves don't do this:
    public void FeedReindeer() { throw new NotSupportedException("Elves don't feed reindeer!"); }
    public void GroomReindeer() { throw new NotSupportedException(); }
    public void TrainReindeer() { throw new NotSupportedException(); }
    public void BakeCookies() { Console.WriteLine("Maybe some elves bake..."); }
    public void DecorateCookies() { Console.WriteLine("Maybe..."); }
    public void RepairSleigh() { throw new NotSupportedException(); }
    public void PolishSleigh() { throw new NotSupportedException(); }
    public void WrapGifts() { Console.WriteLine("Wrapping gifts!"); } // OK
    public void UpdateNaughtyNiceList() { throw new NotSupportedException(); }
}

public class ReindeerCaretaker : IWorkshopWorker
{
    // Reindeer care - these are OK!
    public void FeedReindeer() { Console.WriteLine("Feeding carrots!"); }
    public void GroomReindeer() { Console.WriteLine("Brushing Rudolph!"); }
    public void TrainReindeer() { Console.WriteLine("Flight training!"); }

    // Forced to implement these:
    public void MakeToys() { throw new NotSupportedException("Caretakers don't make toys!"); }
    public void PaintToys() { throw new NotSupportedException(); }
    public void AssembleToys() { throw new NotSupportedException(); }
    public void BakeCookies() { throw new NotSupportedException(); }
    public void DecorateCookies() { throw new NotSupportedException(); }
    public void RepairSleigh() { Console.WriteLine("Maybe help polish..."); }
    public void PolishSleigh() { Console.WriteLine("Polishing sleigh runners!"); }
    public void WrapGifts() { throw new NotSupportedException(); }
    public void UpdateNaughtyNiceList() { throw new NotSupportedException(); }
}

public class Santa : IWorkshopWorker
{
    // Santa manages the list
    public void UpdateNaughtyNiceList() { Console.WriteLine("Checking it twice!"); }

    // But doesn't do the detailed work:
    public void MakeToys() { throw new NotSupportedException("Santa manages, doesn't make!"); }
    public void PaintToys() { throw new NotSupportedException(); }
    public void AssembleToys() { throw new NotSupportedException(); }
    public void FeedReindeer() { Console.WriteLine("Giving treats to Rudolph"); } // Sometimes
    public void GroomReindeer() { throw new NotSupportedException(); }
    public void TrainReindeer() { throw new NotSupportedException(); }
    public void BakeCookies() { Console.WriteLine("Taste testing Mrs. Claus's cookies!"); }
    public void DecorateCookies() { throw new NotSupportedException(); }
    public void RepairSleigh() { throw new NotSupportedException(); }
    public void PolishSleigh() { throw new NotSupportedException(); }
    public void WrapGifts() { throw new NotSupportedException(); }
}

/*
 * ========================================
 * YOUR SOLUTION GOES BELOW THIS LINE
 * ========================================
 *
 * Break the fat interface into focused interfaces:
 *
 * 1. IToyMaker - toy making methods
 * 2. IReindeerCaretaker - reindeer care methods
 * 3. ICookieBaker - baking methods
 * 4. ISleighMechanic - sleigh maintenance methods
 * 5. IGiftWrapper - gift wrapping
 * 6. IListManager - naughty/nice list
 *
 * Then create workers that implement ONLY what they need:
 * - ToyMakerElf : IToyMaker, IGiftWrapper
 * - ReindeerCaretaker : IReindeerCaretaker, ISleighMechanic
 * - MrsClaus : ICookieBaker
 * - HeadElf : IToyMaker, IListManager
 * - Santa : IListManager
 *
 * No more NotSupportedException - workers only have methods they use!
 */

// TODO: Create segregated interfaces here

// TODO: Create improved worker implementations
