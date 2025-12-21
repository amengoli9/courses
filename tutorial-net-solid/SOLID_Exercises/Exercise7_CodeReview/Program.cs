namespace Exercise7_CodeReview;

/// <summary>
/// Exercise 7: Code Review Challenge
/// Run this program to see the problematic code in action.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🔍 Exercise 7: Code Review Challenge 🔍");
        Console.WriteLine("========================================\n");

        Console.WriteLine("This exercise is different - you're a code reviewer!");
        Console.WriteLine("An enthusiastic elf wrote the NorthPoleEmployeeManager class.");
        Console.WriteLine("It works... but it violates almost every SOLID principle!\n");

        Console.WriteLine("========================================");
        Console.WriteLine("YOUR THREE-PART TASK:");
        Console.WriteLine("========================================\n");

        Console.WriteLine("PART 1: Identify ALL SOLID Violations");
        Console.WriteLine("--------------------------------------");
        Console.WriteLine("Go through the code and find every SOLID principle violation.");
        Console.WriteLine("For each violation, document:");
        Console.WriteLine("  - Which principle is violated");
        Console.WriteLine("  - Where it occurs");
        Console.WriteLine("  - Why it's bad");
        Console.WriteLine("  - How to fix it");
        Console.WriteLine("\n🎯 GOAL: Find at least 8 different violations!\n");

        Console.WriteLine("PART 2: Propose a Refactored Design");
        Console.WriteLine("------------------------------------");
        Console.WriteLine("Create a complete class structure showing how you would");
        Console.WriteLine("redesign this to follow ALL SOLID principles.");
        Console.WriteLine("Include interfaces, classes, and their relationships.\n");

        Console.WriteLine("PART 3: Write a Code Review Comment");
        Console.WriteLine("------------------------------------");
        Console.WriteLine("Write a professional, constructive code review comment");
        Console.WriteLine("you would leave for the elf who wrote this code.");
        Console.WriteLine("Be helpful, not harsh!\n");

        Console.WriteLine("========================================");
        Console.WriteLine("GUIDED QUESTIONS:");
        Console.WriteLine("========================================\n");

        Console.WriteLine("SRP Questions:");
        Console.WriteLine("  - How many different reasons would this class have to change?");
        Console.WriteLine("  - Can you list all the jobs this class is doing?");
        Console.WriteLine("  - What happens if the email format changes? PDF format? Bonus rules?\n");

        Console.WriteLine("OCP Questions:");
        Console.WriteLine("  - What happens when Santa adds a new employee type?");
        Console.WriteLine("  - Do you have to modify tested code to add new bonus rules?");
        Console.WriteLine("  - Where are the if-else chains?\n");

        Console.WriteLine("LSP Questions:");
        Console.WriteLine("  - If there were inheritance, could children replace parents?");
        Console.WriteLine("  - Are there problematic class hierarchies?\n");

        Console.WriteLine("ISP Questions:");
        Console.WriteLine("  - Could you split responsibilities into smaller interfaces?");
        Console.WriteLine("  - Would extracted interfaces force unused method implementations?\n");

        Console.WriteLine("DIP Questions:");
        Console.WriteLine("  - Does this class directly create (new) its dependencies?");
        Console.WriteLine("  - Could you easily test this with mock objects?");
        Console.WriteLine("  - Is high-level logic mixed with low-level details?\n");

        Console.WriteLine("========================================");
        Console.WriteLine("EXPECTED FINDINGS CHECKLIST:");
        Console.WriteLine("========================================");
        Console.WriteLine("[ ] Multiple responsibilities in one class (SRP)");
        Console.WriteLine("[ ] Hard-coded database connections (DIP, SRP)");
        Console.WriteLine("[ ] If-else chain for employee types (OCP)");
        Console.WriteLine("[ ] Direct instantiation of dependencies (DIP)");
        Console.WriteLine("[ ] Lack of abstractions/interfaces (DIP)");
        Console.WriteLine("[ ] Mixed business logic and infrastructure (SRP)");
        Console.WriteLine("[ ] Difficult to test (DIP)");
        Console.WriteLine("[ ] Would need modification for new employee types (OCP)");
        Console.WriteLine("[ ] SQL injection vulnerability (bonus finding!)");
        Console.WriteLine("[ ] Multiple database connections in one method (SRP)");

        Console.WriteLine("\n========================================");
        Console.WriteLine("BONUS CHALLENGE:");
        Console.WriteLine("========================================");
        Console.WriteLine("After identifying all violations, implement a fully");
        Console.WriteLine("refactored version that follows all SOLID principles!");

        Console.WriteLine("\n🎅 Good luck, senior elf developer! 🎅");
        Console.WriteLine("\nDocument your findings in the NorthPoleEmployeeManager.cs file!");
    }
}
