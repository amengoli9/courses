# KeyedServices in .NET 10 - Complete Guide and Examples

A comprehensive demonstration of Keyed Services in .NET 10 with real-world examples and best practices.

## 📚 What are Keyed Services?

Keyed Services is a feature in .NET's Dependency Injection container that allows you to register multiple implementations of the same interface and resolve them using a key. This is incredibly useful when you need different implementations based on context, configuration, or runtime conditions.

### Before Keyed Services (The Old Way)

```csharp
// Had to create separate interfaces or use factory patterns
public interface IEmailSender { }
public interface ISmsSender { }
public interface IPushSender { }

// Or use factory pattern with string parameters
public class NotificationFactory
{
    public INotificationSender Create(string type)
    {
        return type switch
        {
            "email" => new EmailSender(),
            "sms" => new SmsSender(),
            _ => throw new ArgumentException()
        };
    }
}
```

### With Keyed Services (The New Way)

```csharp
// Single interface, multiple implementations with keys
services.AddKeyedSingleton<IMessageSender, EmailSender>("email");
services.AddKeyedSingleton<IMessageSender, SmsSender>("sms");
services.AddKeyedSingleton<IMessageSender, PushSender>("push");

// Inject using [FromKeyedServices] attribute
public class NotificationService(
    [FromKeyedServices("email")] IMessageSender emailSender,
    [FromKeyedServices("sms")] IMessageSender smsSender)
{
    // Use the services
}

// Or resolve dynamically
var emailSender = serviceProvider.GetRequiredKeyedService<IMessageSender>("email");
```

## 🎯 Project Structure

```
KeyedServices-Demo/
├── README.md                           # This file
├── KeyedServices.slnx                  # Solution file
│
├── BasicExample/                       # ⭐ Start here - Core concepts
│   ├── BasicExample.csproj
│   └── Program.cs                      # Message senders, storage, cache
│
├── NotificationService/                # 📧 Real-world: Multi-channel notifications
│   ├── NotificationService.csproj
│   └── Program.cs                      # Email, SMS, Push, Slack, Webhook
│
├── PaymentGateway/                     # 💳 Real-world: Multiple payment providers
│   ├── PaymentGateway.csproj
│   └── Program.cs                      # Stripe, PayPal, Square, Crypto
│
├── DatabaseContext/                    # 💾 Real-world: Read/Write separation
│   ├── DatabaseContext.csproj
│   └── Program.cs                      # CQRS, Multi-tenant, Sharding
│
└── ReportingService/                   # 📊 Real-world: Multi-format reports
    ├── ReportingService.csproj
    └── Program.cs                      # PDF, Excel, CSV, JSON, HTML
```

## 🚀 Getting Started

### Prerequisites

- .NET 10.0 SDK or later
- Your favorite IDE (Visual Studio, VS Code, Rider)

### Running the Examples

Each project is a standalone console application:

```bash
# Navigate to any example
cd BasicExample

# Run the example (if dotnet CLI is available)
dotnet run

# Or open KeyedServices.slnx in your IDE and run any project
```

## 📖 Examples Overview

### 1. BasicExample ⭐ **START HERE**

**What it demonstrates:**
- Fundamental Keyed Services concepts
- Three ways to use keyed services
- Basic registration and resolution

**Scenarios:**
- Message Senders (Email, SMS, Push)
- Storage Providers (Local, Cloud, Archive)
- Cache Providers (Memory, Redis)

**Key Concepts:**
```csharp
// Registration
services.AddKeyedSingleton<IMessageSender, EmailSender>("email");

// Injection with attribute
public NotificationService(
    [FromKeyedServices("email")] IMessageSender emailSender)

// Manual resolution
var sender = provider.GetRequiredKeyedService<IMessageSender>("email");
```

---

### 2. NotificationService 📧

**Real-World Scenario:** Multi-channel notification system

**What it demonstrates:**
- Production-ready notification architecture
- Multi-channel delivery (Email, SMS, Push, Slack, Webhook)
- Priority-based routing
- Fallback strategies
- User preferences

**Use Cases:**
- Broadcast messages to all channels
- Urgent notifications via fastest channel
- Critical alerts with redundancy
- User-preferred channel delivery
- Fallback to alternate channels

**Key Features:**
```csharp
// Broadcast to all channels
await orchestrator.SendToAllChannelsAsync(message);

// Send urgent via SMS (fastest)
await orchestrator.SendUrgentAsync(message);

// Fallback strategy
await SendWithFallbackAsync(message, "push", "sms", "email");
```

---

### 3. PaymentGateway 💳

**Real-World Scenario:** Multi-gateway payment processing

**What it demonstrates:**
- Support for multiple payment providers
- Dynamic gateway selection
- Fee comparison and optimization
- Fallback for reliability
- Provider-specific features

**Supported Gateways:**
- Stripe (2.9% fee)
- PayPal (3.5% fee)
- Square (2.6% fee)
- Crypto (1.0% fee)

**Key Features:**
```csharp
// Pay via specific gateway
await paymentService.ProcessPaymentAsync("stripe", payment);

// Find cheapest gateway
await paymentService.ProcessWithLowestFeeAsync(payment);

// Fallback strategy
await ProcessWithFallbackAsync(payment, "stripe", "square", "paypal");

// Compare fees
await processor.CompareGatewayFeesAsync(1000.00m);
```

---

### 4. DatabaseContext 💾

**Real-World Scenario:** Database read/write separation and multi-tenancy

**What it demonstrates:**
- CQRS pattern (Read/Write separation)
- Dedicated analytics database
- Multi-tenant database isolation
- Database sharding patterns

**Database Types:**
- Read Replica (read-only queries)
- Primary Write (write operations)
- Analytics Warehouse (complex analytics)
- Tenant-specific databases

**Key Features:**
```csharp
// Read from replica
await userRepo.GetUserByIdAsync(123);  // Uses read DB

// Write to primary
await userRepo.CreateUserAsync("john", "john@example.com");  // Uses write DB

// Analytics queries
await analyticsService.GenerateReportAsync();  // Uses analytics DB

// Multi-tenant
await multiTenantService.QueryTenantDataAsync("tenant-A", sql);
```

---

### 5. ReportingService 📊

**Real-World Scenario:** Multi-format report generation

**What it demonstrates:**
- Generate reports in multiple formats
- Format-specific optimizations
- Simultaneous multi-format export
- User format preferences

**Supported Formats:**
- PDF (formatted documents)
- Excel (spreadsheets with formulas)
- CSV (data export)
- JSON (API integration)
- HTML (web viewing)

**Key Features:**
```csharp
// Generate specific format
await reportService.GenerateReportAsync("pdf", data);

// Generate all formats
await reportService.GenerateAllFormatsAsync(data);

// Export complete package
await exporter.ExportReportPackageAsync(data);
```

---

## 🎓 Key Concepts and Patterns

### Registration Methods

```csharp
// Singleton (one instance for the lifetime of the app)
services.AddKeyedSingleton<IService, Implementation>("key");

// Scoped (one instance per scope/request)
services.AddKeyedScoped<IService, Implementation>("key");

// Transient (new instance every time)
services.AddKeyedTransient<IService, Implementation>("key");
```

### Resolution Methods

#### 1. Constructor Injection with Attribute (Recommended)

```csharp
public class MyService(
    [FromKeyedServices("key1")] IService service1,
    [FromKeyedServices("key2")] IService service2)
{
    // Use services
}
```

#### 2. Manual Resolution with IServiceProvider

```csharp
public class MyService
{
    private readonly IServiceProvider _provider;

    public MyService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void DoWork(string key)
    {
        var service = _provider.GetRequiredKeyedService<IService>(key);
        service.Execute();
    }
}
```

#### 3. All Implementations at Once

```csharp
// Get all implementations
var allServices = provider.GetKeyedServices<IService>("key");
```

## 💡 Common Use Cases

### 1. Multiple Notification Channels
Register Email, SMS, Push, and other channels as keyed services.

**Benefits:**
- Easy to add new channels
- User preference support
- Fallback strategies
- A/B testing

### 2. Payment Gateway Integration
Support multiple payment providers (Stripe, PayPal, etc.)

**Benefits:**
- Compare fees automatically
- Geographic routing
- Fallback for reliability
- Easy provider switching

### 3. Database Read/Write Separation
CQRS pattern with separate read and write databases.

**Benefits:**
- Performance optimization
- Scalability
- Analytics isolation
- Multi-tenancy

### 4. Multi-Format Export
Generate reports in PDF, Excel, CSV, etc.

**Benefits:**
- User format preferences
- Simultaneous generation
- Format-specific optimizations

### 5. Feature Flags / A/B Testing
Different implementations based on feature flags.

```csharp
services.AddKeyedSingleton<IFeature, FeatureV1>("v1");
services.AddKeyedSingleton<IFeature, FeatureV2>("v2");

// Resolve based on user segment
var feature = provider.GetRequiredKeyedService<IFeature>(userSegment);
```

### 6. Environment-Specific Services
Different implementations for Dev, Staging, Production.

```csharp
services.AddKeyedSingleton<IEmailService, FakeEmailService>("dev");
services.AddKeyedSingleton<IEmailService, RealEmailService>("prod");
```

### 7. Multi-Region Services
Services specific to geographic regions.

```csharp
services.AddKeyedSingleton<IPaymentGateway, StripeUS>("us");
services.AddKeyedSingleton<IPaymentGateway, StripeEU>("eu");
services.AddKeyedSingleton<IPaymentGateway, StripeAsia>("asia");
```

## ✅ Best Practices

### 1. Use Descriptive Keys
```csharp
// ✅ Good - clear and descriptive
services.AddKeyedSingleton<ICache, RedisCache>("redis");
services.AddKeyedSingleton<ICache, MemoryCache>("memory");

// ❌ Bad - unclear
services.AddKeyedSingleton<ICache, RedisCache>("1");
services.AddKeyedSingleton<ICache, MemoryCache>("2");
```

### 2. Use Constants for Keys
```csharp
public static class CacheKeys
{
    public const string Redis = "redis";
    public const string Memory = "memory";
    public const string Distributed = "distributed";
}

// Usage
services.AddKeyedSingleton<ICache, RedisCache>(CacheKeys.Redis);
var cache = provider.GetRequiredKeyedService<ICache>(CacheKeys.Redis);
```

### 3. Combine with Options Pattern
```csharp
services.Configure<EmailOptions>("smtp", options => {
    options.Host = "smtp.example.com";
});

services.AddKeyedSingleton<IEmailSender, SmtpEmailSender>("smtp");
```

### 4. Use Enums for Type-Safe Keys
```csharp
public enum NotificationChannel
{
    Email,
    Sms,
    Push
}

// Register with enum as key
services.AddKeyedSingleton<INotificationSender, EmailSender>(
    NotificationChannel.Email.ToString().ToLower());
```

### 5. Document Your Keys
```csharp
/// <summary>
/// Notification channels available in the system.
/// Keys: "email", "sms", "push", "slack"
/// </summary>
public interface INotificationChannel { }
```

## 🔥 Advanced Patterns

### Dynamic Key Generation

```csharp
public class TenantServiceFactory
{
    private readonly IServiceProvider _provider;

    public TenantServiceFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public ITenantService GetServiceForTenant(string tenantId)
    {
        var key = $"tenant-{tenantId}";
        return _provider.GetRequiredKeyedService<ITenantService>(key);
    }
}
```

### Keyed Services with Factories

```csharp
services.AddKeyedSingleton<IPaymentGateway>("stripe", (provider, key) =>
{
    var options = provider.GetRequiredService<IOptions<StripeOptions>>();
    return new StripePaymentGateway(options.Value);
});
```

### Conditional Registration

```csharp
if (configuration["Environment"] == "Production")
{
    services.AddKeyedSingleton<IEmailService, SendGridService>("email");
}
else
{
    services.AddKeyedSingleton<IEmailService, FakeEmailService>("email");
}
```

## 🆚 Keyed Services vs Alternatives

### vs Named Services (Old Pattern)
```csharp
// Old way - required wrapper interfaces
public interface IEmailNotifier { }
public interface ISmsNotifier { }

// New way - single interface, keyed
services.AddKeyedSingleton<INotifier, EmailNotifier>("email");
services.AddKeyedSingleton<INotifier, SmsNotifier>("sms");
```

### vs Factory Pattern
```csharp
// Old way - manual factory
public class NotifierFactory
{
    public INotifier Create(string type) => type switch
    {
        "email" => new EmailNotifier(),
        "sms" => new SmsNotifier(),
        _ => throw new ArgumentException()
    };
}

// New way - built into DI
services.AddKeyedSingleton<INotifier, EmailNotifier>("email");
var notifier = provider.GetRequiredKeyedService<INotifier>("email");
```

### vs Multiple Interfaces
```csharp
// Old way - interface explosion
public interface IEmailSender { }
public interface ISmsSender { }
public interface IPushSender { }

// New way - one interface, multiple keys
services.AddKeyedSingleton<IMessageSender, EmailSender>("email");
services.AddKeyedSingleton<IMessageSender, SmsSender>("sms");
services.AddKeyedSingleton<IMessageSender, PushSender>("push");
```

## 🎯 When to Use Keyed Services

### ✅ Perfect For:
- Multiple implementations of the same interface
- Runtime selection based on configuration
- Multi-tenant applications
- Feature flags / A/B testing
- Plugin architectures
- Multiple providers (payment, notification, storage)
- Environment-specific implementations

### ❌ Not Ideal For:
- Single implementation scenarios
- When inheritance/polymorphism is more appropriate
- Compile-time type safety is critical
- Very simple applications

## 📚 Additional Resources

- [Official Microsoft Documentation](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#keyed-services)
- [Dependency Injection Best Practices](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines)
- [.NET 8+ What's New](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)

## 🤝 Contributing

Found an issue or have a suggestion? Feel free to contribute!

## 📄 License

This educational material is free to use for learning purposes.

---

**Happy Coding!** 🎉

Made with ❤️ to demonstrate Keyed Services in .NET 10
