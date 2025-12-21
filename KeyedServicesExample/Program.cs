using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Simple KeyedServices Example for .NET 10
///
/// Demonstrates how to register and use multiple implementations of the same interface
/// using Keyed Services - a powerful feature for dependency injection.
///
/// Use Case: A notification system that can send messages via Email, SMS, or Push notifications
/// </summary>

// Step 1: Define the interface that multiple implementations will use
public interface INotificationSender
{
    void Send(string message);
}

// Step 2: Create different implementations
public class EmailNotificationSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($"📧 EMAIL: Sending '{message}' via email");
    }
}

public class SmsNotificationSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($"📱 SMS: Sending '{message}' via SMS");
    }
}

public class PushNotificationSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($"🔔 PUSH: Sending '{message}' via push notification");
    }
}

// Step 3: Create a service that uses keyed services via constructor injection
public class NotificationService
{
    private readonly INotificationSender _emailSender;
    private readonly INotificationSender _smsSender;
    private readonly INotificationSender _pushSender;

    // Use [FromKeyedServices] attribute to inject specific implementations
    public NotificationService(
        [FromKeyedServices("email")] INotificationSender emailSender,
        [FromKeyedServices("sms")] INotificationSender smsSender,
        [FromKeyedServices("push")] INotificationSender pushSender)
    {
        _emailSender = emailSender;
        _smsSender = smsSender;
        _pushSender = pushSender;
    }

    public void SendToAll(string message)
    {
        Console.WriteLine("\n📢 Broadcasting message to all channels:");
        _emailSender.Send(message);
        _smsSender.Send(message);
        _pushSender.Send(message);
    }

    public void SendUrgent(string message)
    {
        Console.WriteLine("\n🚨 Sending urgent notification:");
        _smsSender.Send(message);
        _pushSender.Send(message);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("KEYED SERVICES - SIMPLE EXAMPLE");
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine();

        // Step 4: Configure services and register multiple implementations with keys
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register multiple implementations of INotificationSender with different keys
                services.AddKeyedSingleton<INotificationSender, EmailNotificationSender>("email");
                services.AddKeyedSingleton<INotificationSender, SmsNotificationSender>("sms");
                services.AddKeyedSingleton<INotificationSender, PushNotificationSender>("push");

                // Register the service that uses them
                services.AddSingleton<NotificationService>();
            })
            .Build();

        // Step 5: Use the service
        var notificationService = host.Services.GetRequiredService<NotificationService>();

        // Example 1: Send to all channels
        notificationService.SendToAll("Your order has been shipped!");

        // Example 2: Send urgent notification (SMS + Push only)
        notificationService.SendUrgent("Your verification code is 123456");

        // Example 3: Manually resolve a specific keyed service
        Console.WriteLine("\n🔧 Manual Resolution Example:");
        var emailSender = host.Services.GetRequiredKeyedService<INotificationSender>("email");
        emailSender.Send("Welcome to our service!");

        // Summary
        Console.WriteLine("\n" + "=" .PadRight(60, '='));
        Console.WriteLine("KEY CONCEPTS:");
        Console.WriteLine("=" .PadRight(60, '='));
        Console.WriteLine("✓ Register: services.AddKeyedSingleton<I, Impl>(\"key\")");
        Console.WriteLine("✓ Inject: [FromKeyedServices(\"key\")] I service");
        Console.WriteLine("✓ Resolve: provider.GetRequiredKeyedService<I>(\"key\")");
        Console.WriteLine();
        Console.WriteLine("BENEFITS:");
        Console.WriteLine("✓ Multiple implementations of same interface");
        Console.WriteLine("✓ Select implementation at runtime");
        Console.WriteLine("✓ Clean dependency injection");
        Console.WriteLine("✓ Easy to add new implementations");
        Console.WriteLine();
    }
}
