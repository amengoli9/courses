using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BasicExample;

/// <summary>
/// Basic KeyedServices Example
///
/// KeyedServices allow you to register multiple implementations of the same interface
/// and resolve them using a key. This is useful when you need different implementations
/// based on context or configuration.
/// </summary>

// ========================================
// EXAMPLE 1: Message Sender (Basic Usage)
// ========================================

public interface IMessageSender
{
    void SendMessage(string message);
}

public class EmailSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"📧 EMAIL: {message}");
    }
}

public class SmsSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"📱 SMS: {message}");
    }
}

public class PushNotificationSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"🔔 PUSH: {message}");
    }
}

// ========================================
// EXAMPLE 2: Using [FromKeyedServices] Attribute
// ========================================

public class NotificationService
{
    private readonly IMessageSender _emailSender;
    private readonly IMessageSender _smsSender;
    private readonly IMessageSender _pushSender;

    public NotificationService(
        [FromKeyedServices("email")] IMessageSender emailSender,
        [FromKeyedServices("sms")] IMessageSender smsSender,
        [FromKeyedServices("push")] IMessageSender pushSender)
    {
        _emailSender = emailSender;
        _smsSender = smsSender;
        _pushSender = pushSender;
    }

    public void SendToAll(string message)
    {
        Console.WriteLine("\n📢 Sending to all channels:");
        _emailSender.SendMessage(message);
        _smsSender.SendMessage(message);
        _pushSender.SendMessage(message);
    }

    public void SendUrgent(string message)
    {
        Console.WriteLine("\n🚨 Urgent notification:");
        _smsSender.SendMessage(message);
        _pushSender.SendMessage(message);
    }
}

// ========================================
// EXAMPLE 3: Storage Provider
// ========================================

public interface IStorageProvider
{
    void Save(string data);
    string Load();
}

public class LocalStorageProvider : IStorageProvider
{
    public void Save(string data) => Console.WriteLine($"💾 Saving to LOCAL storage: {data}");
    public string Load() => "Data from local storage";
}

public class CloudStorageProvider : IStorageProvider
{
    public void Save(string data) => Console.WriteLine($"☁️ Saving to CLOUD storage: {data}");
    public string Load() => "Data from cloud storage";
}

public class ArchiveStorageProvider : IStorageProvider
{
    public void Save(string data) => Console.WriteLine($"📦 Saving to ARCHIVE storage: {data}");
    public string Load() => "Data from archive storage";
}

// ========================================
// EXAMPLE 4: Using IServiceProvider directly
// ========================================

public class DataManager
{
    private readonly IServiceProvider _serviceProvider;

    public DataManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void SaveToStorage(string storageType, string data)
    {
        var storage = _serviceProvider.GetRequiredKeyedService<IStorageProvider>(storageType);
        storage.Save(data);
    }

    public void SaveEverywhere(string data)
    {
        Console.WriteLine("\n💾 Saving to all storage providers:");

        var local = _serviceProvider.GetRequiredKeyedService<IStorageProvider>("local");
        var cloud = _serviceProvider.GetRequiredKeyedService<IStorageProvider>("cloud");
        var archive = _serviceProvider.GetRequiredKeyedService<IStorageProvider>("archive");

        local.Save(data);
        cloud.Save(data);
        archive.Save(data);
    }
}

// ========================================
// EXAMPLE 5: Cache Provider
// ========================================

public interface ICacheProvider
{
    void Set(string key, object value);
    object? Get(string key);
}

public class MemoryCacheProvider : ICacheProvider
{
    private readonly Dictionary<string, object> _cache = new();

    public void Set(string key, object value)
    {
        _cache[key] = value;
        Console.WriteLine($"🧠 Memory Cache: Set '{key}'");
    }

    public object? Get(string key)
    {
        Console.WriteLine($"🧠 Memory Cache: Get '{key}'");
        return _cache.GetValueOrDefault(key);
    }
}

public class RedisCacheProvider : ICacheProvider
{
    public void Set(string key, object value)
    {
        Console.WriteLine($"🔴 Redis Cache: Set '{key}' = {value}");
    }

    public object? Get(string key)
    {
        Console.WriteLine($"🔴 Redis Cache: Get '{key}'");
        return $"Redis value for {key}";
    }
}

public class CacheService
{
    private readonly ICacheProvider _memoryCache;
    private readonly ICacheProvider _redisCache;

    public CacheService(
        [FromKeyedServices("memory")] ICacheProvider memoryCache,
        [FromKeyedServices("redis")] ICacheProvider redisCache)
    {
        _memoryCache = memoryCache;
        _redisCache = redisCache;
    }

    public void DemonstrateCaching()
    {
        Console.WriteLine("\n🗄️ Cache Provider Demo:");

        _memoryCache.Set("user:123", "John Doe");
        _redisCache.Set("session:abc", "Active");

        _memoryCache.Get("user:123");
        _redisCache.Get("session:abc");
    }
}

// ========================================
// MAIN PROGRAM
// ========================================

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine("KEYED SERVICES - BASIC EXAMPLES");
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine();

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // ========================================
                // Register Keyed Services
                // ========================================

                // Message Senders with different keys
                services.AddKeyedSingleton<IMessageSender, EmailSender>("email");
                services.AddKeyedSingleton<IMessageSender, SmsSender>("sms");
                services.AddKeyedSingleton<IMessageSender, PushNotificationSender>("push");

                // Storage Providers with different keys
                services.AddKeyedSingleton<IStorageProvider, LocalStorageProvider>("local");
                services.AddKeyedSingleton<IStorageProvider, CloudStorageProvider>("cloud");
                services.AddKeyedSingleton<IStorageProvider, ArchiveStorageProvider>("archive");

                // Cache Providers with different keys
                services.AddKeyedSingleton<ICacheProvider, MemoryCacheProvider>("memory");
                services.AddKeyedSingleton<ICacheProvider, RedisCacheProvider>("redis");

                // Services that use keyed dependencies
                services.AddSingleton<NotificationService>();
                services.AddSingleton<DataManager>();
                services.AddSingleton<CacheService>();
            })
            .Build();

        // ========================================
        // DEMO 1: Notification Service
        // ========================================
        Console.WriteLine("DEMO 1: Notification Service with Multiple Channels");
        Console.WriteLine("-" .PadRight(70, '-'));

        var notificationService = host.Services.GetRequiredService<NotificationService>();
        notificationService.SendToAll("Your order has shipped!");
        notificationService.SendUrgent("System maintenance in 5 minutes");

        // ========================================
        // DEMO 2: Storage Provider
        // ========================================
        Console.WriteLine("\n\nDEMO 2: Storage Provider with Multiple Backends");
        Console.WriteLine("-" .PadRight(70, '-'));

        var dataManager = host.Services.GetRequiredService<DataManager>();
        dataManager.SaveToStorage("local", "User preferences");
        dataManager.SaveToStorage("cloud", "User documents");
        dataManager.SaveToStorage("archive", "Old transactions");
        dataManager.SaveEverywhere("Critical backup data");

        // ========================================
        // DEMO 3: Direct Resolution with IServiceProvider
        // ========================================
        Console.WriteLine("\n\nDEMO 3: Direct Resolution Using IServiceProvider");
        Console.WriteLine("-" .PadRight(70, '-'));

        var emailSender = host.Services.GetRequiredKeyedService<IMessageSender>("email");
        var smsSender = host.Services.GetRequiredKeyedService<IMessageSender>("sms");

        emailSender.SendMessage("Welcome to our service!");
        smsSender.SendMessage("Your verification code is 123456");

        // ========================================
        // DEMO 4: Cache Providers
        // ========================================
        Console.WriteLine("\n\nDEMO 4: Cache Providers");
        Console.WriteLine("-" .PadRight(70, '-'));

        var cacheService = host.Services.GetRequiredService<CacheService>();
        cacheService.DemonstrateCaching();

        // ========================================
        // KEY CONCEPTS SUMMARY
        // ========================================
        Console.WriteLine("\n\n" + "=" .PadRight(70, '='));
        Console.WriteLine("KEY CONCEPTS:");
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine();
        Console.WriteLine("✓ AddKeyedSingleton/Scoped/Transient<TService, TImplementation>(key)");
        Console.WriteLine("  Register multiple implementations with different keys");
        Console.WriteLine();
        Console.WriteLine("✓ [FromKeyedServices(\"key\")] attribute in constructor");
        Console.WriteLine("  Inject specific implementation by key");
        Console.WriteLine();
        Console.WriteLine("✓ GetRequiredKeyedService<T>(key)");
        Console.WriteLine("  Manually resolve service by key from IServiceProvider");
        Console.WriteLine();
        Console.WriteLine("✓ Use Cases:");
        Console.WriteLine("  - Multiple notification channels (Email, SMS, Push)");
        Console.WriteLine("  - Different storage backends (Local, Cloud, Archive)");
        Console.WriteLine("  - Cache providers (Memory, Redis, Distributed)");
        Console.WriteLine("  - Payment gateways (Stripe, PayPal, Square)");
        Console.WriteLine("  - Database contexts (Read, Write, Analytics)");
        Console.WriteLine();
    }
}
