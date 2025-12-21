using System.Data.SqlClient;

namespace Exercise7_CodeReview;

/// <summary>
/// Exercise 7: Code Review Challenge - Find ALL the SOLID Violations!
/// Santa's Elf Manager
///
/// An enthusiastic but inexperienced elf wrote this code to manage employee bonuses
/// at the North Pole. It works... but violates almost every SOLID principle!
///
/// YOUR TASK - THREE-PART ANALYSIS:
///
/// Part 1: Identify ALL SOLID Violations (List & Explain)
/// -------------------------------------------------------
/// Create a detailed list of every SOLID principle violation you can find.
/// For each violation, explain:
/// - Which principle is violated
/// - Where it occurs
/// - Why it's bad
/// - How to fix it
///
/// Part 2: Propose a Refactored Design
/// ------------------------------------
/// Create a complete class diagram or code structure showing how you would
/// restructure this code to follow SOLID principles.
///
/// Part 3: Write a Code Review Comment
/// ------------------------------------
/// Write a professional, constructive code review comment you would leave
/// for the elf who wrote this code.
///
/// HINT: You should find at least 8 different violations!
/// </summary>
public class NorthPoleEmployeeManager
{
    public void ProcessEmployeeBonus(int employeeId)
    {
        // Get employee from North Pole database
        var connection = new SqlConnection("Server=NorthPole;Database=SantaWorkshop;");
        connection.Open();
        var command = new SqlCommand(
            $"SELECT * FROM Employees WHERE Id = {employeeId}", connection);
        var reader = command.ExecuteReader();

        if (reader.Read())
        {
            string name = reader["Name"].ToString() ?? "";
            string type = reader["EmployeeType"].ToString() ?? "";
            decimal salary = Convert.ToDecimal(reader["Salary"]);

            // Calculate Christmas bonus based on employee type
            decimal bonus = 0;
            if (type == "HeadElf")
            {
                bonus = salary * 0.25m; // 25% for head elves
            }
            else if (type == "ToyMaker")
            {
                bonus = salary * 0.20m; // 20% for toy makers
            }
            else if (type == "ReindeerCaretaker")
            {
                bonus = salary * 0.15m; // 15% for reindeer care
            }
            else if (type == "CookieBaker")
            {
                bonus = salary * 0.18m; // 18% for Mrs. Claus's team
            }
            else if (type == "ListManager")
            {
                bonus = salary * 0.22m; // 22% for naughty/nice list
            }

            // Update salary with bonus in database
            var updateCommand = new SqlCommand(
                $"UPDATE Employees SET Salary = {salary + bonus} WHERE Id = {employeeId}",
                connection);
            updateCommand.ExecuteNonQuery();

            // Send notification letter via chimney mail
            var chimneyMail = new ChimneyMailService();
            var letter = new Letter();
            letter.To.Add($"{name}@northpole.christmas");
            letter.Subject = "🎄 Christmas Bonus Applied!";
            letter.Body = $"Dear {name},\n\n" +
                         $"Your Christmas bonus of ${bonus:F2} has been applied!\n" +
                         $"New salary: ${salary + bonus:F2}\n\n" +
                         $"Ho Ho Ho!\n" +
                         $"- Santa's Payroll Department";
            chimneyMail.SendViaReindeer(letter);

            // Log to Santa's Big Book of Records
            File.AppendAllText("ChristmasBonusLog.txt",
                $"{DateTime.Now}: Bonus of ${bonus:F2} applied to {name} ({type})\n");

            // Generate bonus certificate PDF
            var pdfMaker = new PdfDocument();
            pdfMaker.AddPage("North Pole Bonus Certificate");
            pdfMaker.AddText($"Employee: {name}");
            pdfMaker.AddText($"Position: {type}");
            pdfMaker.AddText($"Christmas Bonus: ${bonus:F2}");
            pdfMaker.AddText($"Approved by: Santa Claus");
            pdfMaker.AddImage("santa_signature.png");
            pdfMaker.Save($"bonus_certificate_{employeeId}.pdf");

            // Update workshop production metrics
            if (type == "ToyMaker")
            {
                var metricsDb = new SqlConnection("Server=NorthPole;Database=WorkshopMetrics;");
                metricsDb.Open();
                var metricsCmd = new SqlCommand(
                    $"UPDATE ToyMakerMetrics SET BonusPaid = 1 WHERE ElfId = {employeeId}",
                    metricsDb);
                metricsCmd.ExecuteNonQuery();
                metricsDb.Close();
            }

            // Send SMS to Mrs. Claus if bonus is over $1000
            if (bonus > 1000)
            {
                var smsService = new SmsService();
                smsService.SendText("555-CLAUS",
                    $"Large bonus alert: ${bonus:F2} paid to {name}");
            }
        }

        connection.Close();
    }
}

// Supporting classes (simplified)
public class ChimneyMailService
{
    public void SendViaReindeer(Letter letter)
    {
        Console.WriteLine($"📬 Sending letter to: {string.Join(", ", letter.To)}");
    }
}

public class Letter
{
    public List<string> To { get; set; } = new();
    public string Subject { get; set; } = "";
    public string Body { get; set; } = "";
}

public class PdfDocument
{
    private List<string> content = new();

    public void AddPage(string title)
    {
        content.Add($"=== {title} ===");
    }

    public void AddText(string text)
    {
        content.Add(text);
    }

    public void AddImage(string imagePath)
    {
        content.Add($"[Image: {imagePath}]");
    }

    public void Save(string filename)
    {
        Console.WriteLine($"📄 Saving PDF: {filename}");
        foreach (var line in content)
        {
            Console.WriteLine($"   {line}");
        }
    }
}

public class SmsService
{
    public void SendText(string phoneNumber, string message)
    {
        Console.WriteLine($"📱 SMS to {phoneNumber}: {message}");
    }
}

/*
 * ========================================
 * YOUR ANALYSIS GOES HERE
 * ========================================
 *
 * PART 1: List ALL SOLID Violations
 * ----------------------------------
 *
 * VIOLATION #1 - Single Responsibility Principle
 * Location: [Specify]
 * Problem: [Explain what's wrong]
 * Why it's bad: [Consequences]
 * How to fix: [Solution]
 *
 * VIOLATION #2 - [Which principle?]
 * ...
 *
 * [Continue for ALL violations - find at least 8!]
 *
 *
 * PART 2: Proposed Refactored Design
 * -----------------------------------
 *
 * [Write your proposed class structure here]
 * Example:
 * public interface IBonusCalculator { ... }
 * public interface IEmployeeRepository { ... }
 * public class NorthPoleEmployeeService
 * {
 *     public NorthPoleEmployeeService(
 *         IBonusCalculator bonusCalculator,
 *         IEmployeeRepository employeeRepository,
 *         ...)
 *     { }
 * }
 *
 *
 * PART 3: Code Review Comment
 * ----------------------------
 *
 * [Write your professional, constructive code review comment here]
 *
 */
