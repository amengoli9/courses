using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NotificationService;

/// <summary>
/// Real-World Example: Multi-Channel Notification System
///
/// This example demonstrates a production-ready notification system that can send
/// messages through different channels (Email, SMS, Push, Slack) using KeyedServices.
/// </summary>

// ========================================
// DOMAIN MODELS
// ========================================

public record NotificationMessage(
    string RecipientId,
    string Subject,
    string Body,
    NotificationPriority Priority = NotificationPriority.Normal,
    Dictionary<string, string>? Metadata = null
);

public enum NotificationPriority
{
    Low,
    Normal,
    High,
    Urgent
}

public record NotificationResult(
    bool Success,
    string Channel,
    string? MessageId = null,
    string? ErrorMessage = null
);

// ========================================
// NOTIFICATION CHANNEL ABSTRACTION
// ========================================

public interface INotificationChannel
{
    string ChannelName { get; }
    Task<NotificationResult> SendAsync(NotificationMessage message);
    bool SupportsAttachments { get; }
    int MaxMessageLength { get; }
}

// ========================================
// CONCRETE NOTIFICATION CHANNELS
// ========================================

public class EmailNotificationChannel : INotificationChannel
{
    public string ChannelName => "Email";
    public bool SupportsAttachments => true;
    public int MaxMessageLength => int.MaxValue;

    public async Task<NotificationResult> SendAsync(NotificationMessage message)
    {
        Console.WriteLine($"📧 [EMAIL] Sending to: {message.RecipientId}");
        Console.WriteLine($"   Subject: {message.Subject}");
        Console.WriteLine($"   Priority: {message.Priority}");

        // Simulate email sending
        await Task.Delay(100);

        var messageId = $"email-{Guid.NewGuid():N}";
        Console.WriteLine($"   ✓ Sent successfully (ID: {messageId})");

        return new NotificationResult(true, ChannelName, messageId);
    }
}

public class SmsNotificationChannel : INotificationChannel
{
    public string ChannelName => "SMS";
    public bool SupportsAttachments => false;
    public int MaxMessageLength => 160;

    public async Task<NotificationResult> SendAsync(NotificationMessage message)
    {
        Console.WriteLine($"📱 [SMS] Sending to: {message.RecipientId}");

        var truncatedBody = message.Body.Length > MaxMessageLength
            ? message.Body[..MaxMessageLength] + "..."
            : message.Body;

        Console.WriteLine($"   Message: {truncatedBody}");
        Console.WriteLine($"   Priority: {message.Priority}");

        // Simulate SMS sending
        await Task.Delay(50);

        var messageId = $"sms-{Guid.NewGuid():N}";
        Console.WriteLine($"   ✓ Sent successfully (ID: {messageId})");

        return new NotificationResult(true, ChannelName, messageId);
    }
}

public class PushNotificationChannel : INotificationChannel
{
    public string ChannelName => "Push Notification";
    public bool SupportsAttachments => false;
    public int MaxMessageLength => 256;

    public async Task<NotificationResult> SendAsync(NotificationMessage message)
    {
        Console.WriteLine($"🔔 [PUSH] Sending to: {message.RecipientId}");
        Console.WriteLine($"   Title: {message.Subject}");
        Console.WriteLine($"   Body: {message.Body}");
        Console.WriteLine($"   Priority: {message.Priority}");

        // Simulate push notification
        await Task.Delay(30);

        var messageId = $"push-{Guid.NewGuid():N}";
        Console.WriteLine($"   ✓ Sent successfully (ID: {messageId})");

        return new NotificationResult(true, ChannelName, messageId);
    }
}

public class SlackNotificationChannel : INotificationChannel
{
    public string ChannelName => "Slack";
    public bool SupportsAttachments => true;
    public int MaxMessageLength => 4000;

    public async Task<NotificationResult> SendAsync(NotificationMessage message)
    {
        Console.WriteLine($"💬 [SLACK] Posting to channel");
        Console.WriteLine($"   User: {message.RecipientId}");
        Console.WriteLine($"   Message: {message.Subject}");
        Console.WriteLine($"   {message.Body}");

        // Simulate Slack API call
        await Task.Delay(75);

        var messageId = $"slack-{Guid.NewGuid():N}";
        Console.WriteLine($"   ✓ Posted successfully (ID: {messageId})");

        return new NotificationResult(true, ChannelName, messageId);
    }
}

public class WebhookNotificationChannel : INotificationChannel
{
    public string ChannelName => "Webhook";
    public bool SupportsAttachments => true;
    public int MaxMessageLength => int.MaxValue;

    public async Task<NotificationResult> SendAsync(NotificationMessage message)
    {
        Console.WriteLine($"🔗 [WEBHOOK] Sending HTTP POST");
        Console.WriteLine($"   Endpoint: https://api.example.com/notifications");
        Console.WriteLine($"   Payload: {message.Subject}");

        // Simulate webhook call
        await Task.Delay(120);

        var messageId = $"webhook-{Guid.NewGuid():N}";
        Console.WriteLine($"   ✓ Webhook called successfully (ID: {messageId})");

        return new NotificationResult(true, ChannelName, messageId);
    }
}

// ========================================
// NOTIFICATION ORCHESTRATOR
// ========================================

public class NotificationOrchestrator
{
    private readonly INotificationChannel _emailChannel;
    private readonly INotificationChannel _smsChannel;
    private readonly INotificationChannel _pushChannel;
    private readonly INotificationChannel _slackChannel;
    private readonly INotificationChannel _webhookChannel;

    public NotificationOrchestrator(
        [FromKeyedServices("email")] INotificationChannel emailChannel,
        [FromKeyedServices("sms")] INotificationChannel smsChannel,
        [FromKeyedServices("push")] INotificationChannel pushChannel,
        [FromKeyedServices("slack")] INotificationChannel slackChannel,
        [FromKeyedServices("webhook")] INotificationChannel webhookChannel)
    {
        _emailChannel = emailChannel;
        _smsChannel = smsChannel;
        _pushChannel = pushChannel;
        _slackChannel = slackChannel;
        _webhookChannel = webhookChannel;
    }

    public async Task<List<NotificationResult>> SendToAllChannelsAsync(NotificationMessage message)
    {
        Console.WriteLine($"\n📣 Broadcasting message to ALL channels:");
        Console.WriteLine(new string('=', 70));

        var tasks = new[]
        {
            _emailChannel.SendAsync(message),
            _smsChannel.SendAsync(message),
            _pushChannel.SendAsync(message),
            _slackChannel.SendAsync(message),
            _webhookChannel.SendAsync(message)
        };

        var results = await Task.WhenAll(tasks);
        return results.ToList();
    }

    public async Task<NotificationResult> SendUrgentAsync(NotificationMessage message)
    {
        Console.WriteLine($"\n🚨 URGENT notification - using fastest channel (SMS):");
        Console.WriteLine(new string('=', 70));

        return await _smsChannel.SendAsync(message);
    }

    public async Task<NotificationResult> SendMarketingAsync(NotificationMessage message)
    {
        Console.WriteLine($"\n📈 Marketing notification - using Email:");
        Console.WriteLine(new string('=', 70));

        return await _emailChannel.SendAsync(message);
    }

    public async Task<List<NotificationResult>> SendCriticalAlertAsync(NotificationMessage message)
    {
        Console.WriteLine($"\n⚠️ CRITICAL ALERT - Multi-channel delivery:");
        Console.WriteLine(new string('=', 70));

        // Send through SMS, Push, and Slack for critical alerts
        var tasks = new[]
        {
            _smsChannel.SendAsync(message),
            _pushChannel.SendAsync(message),
            _slackChannel.SendAsync(message)
        };

        var results = await Task.WhenAll(tasks);
        return results.ToList();
    }
}

// ========================================
// USER PREFERENCE SERVICE
// ========================================

public class UserPreferenceService
{
    private readonly IServiceProvider _serviceProvider;

    public UserPreferenceService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<NotificationResult> SendByUserPreferenceAsync(
        string userId,
        string preferredChannel,
        NotificationMessage message)
    {
        Console.WriteLine($"\n👤 Sending via user's preferred channel: {preferredChannel}");
        Console.WriteLine(new string('=', 70));

        var channel = _serviceProvider.GetRequiredKeyedService<INotificationChannel>(preferredChannel);
        return await channel.SendAsync(message);
    }

    public async Task<List<NotificationResult>> SendWithFallbackAsync(
        NotificationMessage message,
        params string[] channelKeys)
    {
        Console.WriteLine($"\n🔄 Sending with fallback strategy:");
        Console.WriteLine(new string('=', 70));

        var results = new List<NotificationResult>();

        foreach (var channelKey in channelKeys)
        {
            try
            {
                var channel = _serviceProvider.GetRequiredKeyedService<INotificationChannel>(channelKey);
                var result = await channel.SendAsync(message);

                results.Add(result);

                if (result.Success)
                {
                    Console.WriteLine($"   ✓ Successfully sent via {channelKey}");
                    break; // Stop after first successful send
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ Failed to send via {channelKey}: {ex.Message}");
                results.Add(new NotificationResult(false, channelKey, ErrorMessage: ex.Message));
            }
        }

        return results;
    }
}

// ========================================
// MAIN PROGRAM
// ========================================

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine("REAL-WORLD KEYED SERVICES: NOTIFICATION SYSTEM");
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine();

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register all notification channels as keyed services
                services.AddKeyedSingleton<INotificationChannel, EmailNotificationChannel>("email");
                services.AddKeyedSingleton<INotificationChannel, SmsNotificationChannel>("sms");
                services.AddKeyedSingleton<INotificationChannel, PushNotificationChannel>("push");
                services.AddKeyedSingleton<INotificationChannel, SlackNotificationChannel>("slack");
                services.AddKeyedSingleton<INotificationChannel, WebhookNotificationChannel>("webhook");

                // Register orchestrators
                services.AddSingleton<NotificationOrchestrator>();
                services.AddSingleton<UserPreferenceService>();
            })
            .Build();

        var orchestrator = host.Services.GetRequiredService<NotificationOrchestrator>();
        var userPreferenceService = host.Services.GetRequiredService<UserPreferenceService>();

        // ========================================
        // SCENARIO 1: Broadcast to all channels
        // ========================================
        var broadcastMessage = new NotificationMessage(
            RecipientId: "user@example.com",
            Subject: "System Maintenance Scheduled",
            Body: "Our system will undergo maintenance on Sunday from 2 AM to 6 AM EST.",
            Priority: NotificationPriority.High
        );

        await orchestrator.SendToAllChannelsAsync(broadcastMessage);

        // ========================================
        // SCENARIO 2: Urgent notification
        // ========================================
        var urgentMessage = new NotificationMessage(
            RecipientId: "+1234567890",
            Subject: "Security Alert",
            Body: "Unusual login detected from new device. If this wasn't you, reset your password immediately.",
            Priority: NotificationPriority.Urgent
        );

        await orchestrator.SendUrgentAsync(urgentMessage);

        // ========================================
        // SCENARIO 3: Marketing email
        // ========================================
        var marketingMessage = new NotificationMessage(
            RecipientId: "customer@example.com",
            Subject: "20% Off Your Next Purchase!",
            Body: "Thank you for being a valued customer. Use code SAVE20 at checkout.",
            Priority: NotificationPriority.Low
        );

        await orchestrator.SendMarketingAsync(marketingMessage);

        // ========================================
        // SCENARIO 4: Critical alert
        // ========================================
        var criticalMessage = new NotificationMessage(
            RecipientId: "admin@example.com",
            Subject: "Production Database Error",
            Body: "Database connection pool exhausted. Immediate action required!",
            Priority: NotificationPriority.Urgent
        );

        await orchestrator.SendCriticalAlertAsync(criticalMessage);

        // ========================================
        // SCENARIO 5: User preference
        // ========================================
        var preferenceMessage = new NotificationMessage(
            RecipientId: "user@example.com",
            Subject: "Order Shipped",
            Body: "Your order #12345 has been shipped and will arrive in 2-3 business days.",
            Priority: NotificationPriority.Normal
        );

        await userPreferenceService.SendByUserPreferenceAsync(
            "user123",
            "email", // User's preferred channel
            preferenceMessage
        );

        // ========================================
        // SCENARIO 6: Fallback strategy
        // ========================================
        var fallbackMessage = new NotificationMessage(
            RecipientId: "user@example.com",
            Subject: "Important Account Update",
            Body: "Your account settings have been updated.",
            Priority: NotificationPriority.Normal
        );

        await userPreferenceService.SendWithFallbackAsync(
            fallbackMessage,
            "push",    // Try push first
            "sms",     // Fall back to SMS
            "email"    // Finally fall back to email
        );

        // ========================================
        // SUMMARY
        // ========================================
        Console.WriteLine("\n\n" + "=" .PadRight(70, '='));
        Console.WriteLine("BENEFITS OF KEYED SERVICES FOR NOTIFICATIONS:");
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine();
        Console.WriteLine("✓ Easy to add new notification channels");
        Console.WriteLine("✓ Clean separation of concerns");
        Console.WriteLine("✓ Flexible routing based on user preferences");
        Console.WriteLine("✓ Support for fallback strategies");
        Console.WriteLine("✓ Priority-based channel selection");
        Console.WriteLine("✓ All channels implement same interface");
        Console.WriteLine("✓ Testable with mock channels");
        Console.WriteLine();
    }
}
