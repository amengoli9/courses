using System.Data.SqlClient;

namespace Exercise1_SRP;

/// <summary>
/// Exercise 1: Single Responsibility Principle (SRP)
/// Santa's Elf Manager Problem
///
/// PROBLEM: This class does EVERYTHING for managing toy requests.
/// Refactor it to follow SRP where each class has ONE job in Santa's workshop.
///
/// YOUR TASK:
/// 1. List all the different jobs this elf class is doing
/// 2. Create separate elf specialist classes for each responsibility
/// 3. Refactor so each class has ONE job
///
/// HINT: Create these specialized elf classes:
/// - NiceListValidator (checks if request is valid)
/// - ToyInventoryChecker (manages toy availability)
/// - WorkshopRepository (saves to North Pole database)
/// - ChimneyMailService (sends letters to children)
/// - SantaLogger (maintains the Big Book)
/// - WorkshopQueueManager (manages production queue)
/// - ToyRequestCoordinator (orchestrates all the elves)
/// </summary>
public class ToyRequestManager
{
    public void ProcessToyRequest(string childName, string address, string toyName, int age)
    {
        // Validate if child is on nice list
        if (string.IsNullOrEmpty(childName))
            throw new ArgumentException("Child name is required");
        if (age < 0 || age > 18)
            throw new ArgumentException("Invalid age for toy request");
        if (string.IsNullOrEmpty(toyName))
            throw new ArgumentException("Toy name is required");

        // Check inventory for toy availability
        bool toyAvailable = CheckInventory(toyName);

        // Save to North Pole database
        using (var connection = new SqlConnection("Server=NorthPole;Database=SantaWorkshop;"))
        {
            connection.Open();
            var command = new SqlCommand("INSERT INTO ToyRequests...", connection);
            command.Parameters.AddWithValue("@childName", childName);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@toyName", toyName);
            command.Parameters.AddWithValue("@age", age);
            command.ExecuteNonQuery();
        }

        // Send confirmation letter to child
        var mailService = new ChimneyMailService();
        var letter = new Letter();
        letter.To.Add(address);
        letter.Subject = "Your Toy Request Received!";
        letter.Body = $"Dear {childName}, Santa has received your request for {toyName}!";
        mailService.SendViaReindeer(letter);

        // Log in Santa's Big Book
        File.AppendAllText("NaughtyNiceLog.txt",
            $"{DateTime.Now}: {childName} requested {toyName}\n");

        // Calculate workshop capacity
        UpdateWorkshopQueue(toyName);
    }

    private bool CheckInventory(string toyName)
    {
        // Inventory logic
        return true; // Simplified
    }

    private void UpdateWorkshopQueue(string toyName)
    {
        // Queue logic
    }
}

// Supporting classes (simplified for the exercise)
public class ChimneyMailService
{
    public void SendViaReindeer(Letter letter)
    {
        Console.WriteLine($"Sending letter via reindeer to: {string.Join(", ", letter.To)}");
    }
}

public class Letter
{
    public List<string> To { get; set; } = new();
    public string Subject { get; set; } = "";
    public string Body { get; set; } = "";
}

/*
 * ========================================
 * YOUR SOLUTION GOES BELOW THIS LINE
 * ========================================
 *
 * Create the following classes following SRP:
 *
 * 1. NiceListValidator - Validates child information and requests
 * 2. ToyInventoryChecker - Checks workshop inventory
 * 3. WorkshopRepository - Handles database operations
 * 4. SantaLogger - Maintains the Big Book
 * 5. WorkshopQueueManager - Manages production queue
 * 6. ToyRequestCoordinator - Orchestrates all the specialists
 *
 * Each class should have a single, well-defined responsibility!
 */
