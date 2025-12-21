# KeyedServices Simple Example (.NET 10)

A simple, easy-to-understand demonstration of Keyed Services in .NET 10.

## What are Keyed Services?

Keyed Services allow you to register multiple implementations of the same interface and resolve them using a key. This is useful when you need different implementations based on context.

## Example Scenario

This example demonstrates a notification system that can send messages through different channels:
- 📧 Email
- 📱 SMS
- 🔔 Push Notifications

## How It Works

### 1. Define the Interface

```csharp
public interface INotificationSender
{
    void Send(string message);
}
```

### 2. Create Implementations

```csharp
public class EmailNotificationSender : INotificationSender { }
public class SmsNotificationSender : INotificationSender { }
public class PushNotificationSender : INotificationSender { }
```

### 3. Register with Keys

```csharp
services.AddKeyedSingleton<INotificationSender, EmailNotificationSender>("email");
services.AddKeyedSingleton<INotificationSender, SmsNotificationSender>("sms");
services.AddKeyedSingleton<INotificationSender, PushNotificationSender>("push");
```

### 4. Inject Using Attribute

```csharp
public class NotificationService(
    [FromKeyedServices("email")] INotificationSender emailSender,
    [FromKeyedServices("sms")] INotificationSender smsSender,
    [FromKeyedServices("push")] INotificationSender pushSender)
{
    // Use the services
}
```

### 5. Or Resolve Manually

```csharp
var emailSender = serviceProvider.GetRequiredKeyedService<INotificationSender>("email");
```

## Running the Example

```bash
# Navigate to the project directory
cd KeyedServicesExample

# Run the project
dotnet run
```

## Expected Output

```
============================================================
KEYED SERVICES - SIMPLE EXAMPLE
============================================================

📢 Broadcasting message to all channels:
📧 EMAIL: Sending 'Your order has been shipped!' via email
📱 SMS: Sending 'Your order has been shipped!' via SMS
🔔 PUSH: Sending 'Your order has been shipped!' via push notification

🚨 Sending urgent notification:
📱 SMS: Sending 'Your verification code is 123456' via SMS
🔔 PUSH: Sending 'Your verification code is 123456' via push notification

🔧 Manual Resolution Example:
📧 EMAIL: Sending 'Welcome to our service!' via email

============================================================
KEY CONCEPTS:
============================================================
✓ Register: services.AddKeyedSingleton<I, Impl>("key")
✓ Inject: [FromKeyedServices("key")] I service
✓ Resolve: provider.GetRequiredKeyedService<I>("key")

BENEFITS:
✓ Multiple implementations of same interface
✓ Select implementation at runtime
✓ Clean dependency injection
✓ Easy to add new implementations
```

## Key Takeaways

1. **Register**: Use `AddKeyedSingleton/Scoped/Transient<TInterface, TImplementation>(key)`
2. **Inject**: Use `[FromKeyedServices("key")]` attribute in constructors
3. **Resolve**: Use `GetRequiredKeyedService<T>(key)` for manual resolution

## When to Use Keyed Services

- ✅ Multiple implementations of the same interface
- ✅ Runtime selection based on configuration or context
- ✅ Multi-channel systems (notifications, payments, etc.)
- ✅ Feature flags / A/B testing
- ✅ Multi-tenant applications

## Learn More

- [Microsoft Documentation](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#keyed-services)
- [.NET 8+ What's New](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
