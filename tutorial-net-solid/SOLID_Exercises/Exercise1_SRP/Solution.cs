namespace Exercise1_SRP.Solution;

/// <summary>
/// SOLUTION: Single Responsibility Principle
/// Each class now has ONE clear responsibility and ONE reason to change.
/// </summary>

// ========================================
// 1. VALIDATION - Single Responsibility: Validate toy requests
// ========================================
public class NiceListValidator
{
    public void ValidateRequest(string childName, string toyName, int age)
    {
        if (string.IsNullOrEmpty(childName))
            throw new ArgumentException("Child name is required");

        if (age < 0 || age > 18)
            throw new ArgumentException("Invalid age for toy request");

        if (string.IsNullOrEmpty(toyName))
            throw new ArgumentException("Toy name is required");
    }
}

// ========================================
// 2. INVENTORY - Single Responsibility: Check toy availability
// ========================================
public class ToyInventoryChecker
{
    public bool IsToyAvailable(string toyName)
    {
        // In a real system, this would check actual inventory
        Console.WriteLine($"✓ Checking inventory for {toyName}");
        return true; // Simplified
    }
}

// ========================================
// 3. DATABASE - Single Responsibility: Handle data persistence
// ========================================
public interface IWorkshopRepository
{
    void SaveToyRequest(string childName, string address, string toyName, int age);
}

public class WorkshopRepository : IWorkshopRepository
{
    public void SaveToyRequest(string childName, string address, string toyName, int age)
    {
        // In a real system, this would use actual database operations
        Console.WriteLine($"✓ Saved to North Pole database: {childName} -> {toyName}");

        // Simulated database save
        // using (var connection = new SqlConnection("Server=NorthPole;Database=SantaWorkshop;"))
        // {
        //     connection.Open();
        //     var command = new SqlCommand("INSERT INTO ToyRequests...", connection);
        //     command.Parameters.AddWithValue("@childName", childName);
        //     command.Parameters.AddWithValue("@address", address);
        //     command.Parameters.AddWithValue("@toyName", toyName);
        //     command.Parameters.AddWithValue("@age", age);
        //     command.ExecuteNonQuery();
        // }
    }
}

// ========================================
// 4. MAIL SERVICE - Single Responsibility: Send letters to children
// ========================================
public interface IChimneyMailService
{
    void SendConfirmationLetter(string childName, string address, string toyName);
}

public class ChimneyMailService : IChimneyMailService
{
    public void SendConfirmationLetter(string childName, string address, string toyName)
    {
        var letter = new Letter
        {
            To = new List<string> { address },
            Subject = "Your Toy Request Received!",
            Body = $"Dear {childName}, Santa has received your request for {toyName}!"
        };

        Console.WriteLine($"✓ Sent letter to {childName} at {address}");
        SendViaReindeer(letter);
    }

    private void SendViaReindeer(Letter letter)
    {
        // Actual mail sending logic
        Console.WriteLine($"  📬 Letter dispatched via reindeer express");
    }
}

// ========================================
// 5. LOGGING - Single Responsibility: Maintain Santa's records
// ========================================
public interface ISantaLogger
{
    void LogToyRequest(string childName, string toyName);
}

public class SantaLogger : ISantaLogger
{
    private readonly string _logFilePath;

    public SantaLogger(string logFilePath = "NaughtyNiceLog.txt")
    {
        _logFilePath = logFilePath;
    }

    public void LogToyRequest(string childName, string toyName)
    {
        var logEntry = $"{DateTime.Now}: {childName} requested {toyName}\n";
        // In real implementation: File.AppendAllText(_logFilePath, logEntry);
        Console.WriteLine($"✓ Logged to Santa's Big Book: {childName} -> {toyName}");
    }
}

// ========================================
// 6. QUEUE MANAGEMENT - Single Responsibility: Manage production queue
// ========================================
public interface IWorkshopQueueManager
{
    void AddToProductionQueue(string toyName);
}

public class WorkshopQueueManager : IWorkshopQueueManager
{
    public void AddToProductionQueue(string toyName)
    {
        // Workshop capacity calculation and queue management
        Console.WriteLine($"✓ Added {toyName} to production queue");
        Console.WriteLine($"  🏭 Workshop elves notified");
    }
}

// ========================================
// 7. COORDINATOR - Single Responsibility: Orchestrate the workflow
// ========================================
public class ToyRequestCoordinator
{
    private readonly NiceListValidator _validator;
    private readonly ToyInventoryChecker _inventoryChecker;
    private readonly IWorkshopRepository _repository;
    private readonly IChimneyMailService _mailService;
    private readonly ISantaLogger _logger;
    private readonly IWorkshopQueueManager _queueManager;

    public ToyRequestCoordinator(
        NiceListValidator validator,
        ToyInventoryChecker inventoryChecker,
        IWorkshopRepository repository,
        IChimneyMailService mailService,
        ISantaLogger logger,
        IWorkshopQueueManager queueManager)
    {
        _validator = validator;
        _inventoryChecker = inventoryChecker;
        _repository = repository;
        _mailService = mailService;
        _logger = logger;
        _queueManager = queueManager;
    }

    public void ProcessToyRequest(string childName, string address, string toyName, int age)
    {
        Console.WriteLine($"\n🎅 Processing toy request from {childName}...\n");

        // Step 1: Validate
        _validator.ValidateRequest(childName, toyName, age);

        // Step 2: Check inventory
        bool available = _inventoryChecker.IsToyAvailable(toyName);
        if (!available)
        {
            Console.WriteLine($"✗ {toyName} is out of stock!");
            return;
        }

        // Step 3: Save to database
        _repository.SaveToyRequest(childName, address, toyName, age);

        // Step 4: Send confirmation
        _mailService.SendConfirmationLetter(childName, address, toyName);

        // Step 5: Log the request
        _logger.LogToyRequest(childName, toyName);

        // Step 6: Add to production queue
        _queueManager.AddToProductionQueue(toyName);

        Console.WriteLine($"\n✅ Request processed successfully for {childName}!\n");
    }
}

// ========================================
// DEMONSTRATION
// ========================================
public class SrpSolutionDemo
{
    public static void Run()
    {
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("SOLUTION: Single Responsibility Principle");
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine();

        // Create all the specialist classes
        var validator = new NiceListValidator();
        var inventoryChecker = new ToyInventoryChecker();
        var repository = new WorkshopRepository();
        var mailService = new ChimneyMailService();
        var logger = new SantaLogger();
        var queueManager = new WorkshopQueueManager();

        // Create the coordinator with all dependencies
        var coordinator = new ToyRequestCoordinator(
            validator,
            inventoryChecker,
            repository,
            mailService,
            logger,
            queueManager
        );

        // Process a toy request
        coordinator.ProcessToyRequest(
            childName: "Emma",
            address: "123 Candy Cane Lane, Winter Wonderland",
            toyName: "Teddy Bear",
            age: 8
        );

        Console.WriteLine("\n" + "=" .PadRight(60, '='));
        Console.WriteLine("BENEFITS OF SRP:");
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("✓ Each class has ONE clear responsibility");
        Console.WriteLine("✓ Easy to test - can mock each dependency");
        Console.WriteLine("✓ Easy to maintain - changes are isolated");
        Console.WriteLine("✓ Easy to understand - clear class names");
        Console.WriteLine("✓ Easy to reuse - components are independent");
        Console.WriteLine();
    }
}
