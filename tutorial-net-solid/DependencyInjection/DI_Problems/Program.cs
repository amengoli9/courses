using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Console.WriteLine("=".PadRight(80, '='));
Console.WriteLine("DI PROBLEMS IN .NET 10 - CAPTIVE DEPENDENCY & SCOPED IN HOSTEDSERVICE");
Console.WriteLine("=".PadRight(80, '='));

// Uncomment one of the following to run the example:

// PROBLEM 1: Captive Dependency
//await CaptiveDependencyProblem.RunAsync();
// await CaptiveDependencySolution.RunAsync();

// PROBLEM 2: Scoped Service in IHostedService
 await ScopedInHostedServiceProblem.RunAsync();
//await ScopedInHostedServiceSolution.RunAsync();


// ============================================================================
// PROBLEM 1: CAPTIVE DEPENDENCY
// ============================================================================
// A "captive dependency" occurs when a longer-lived service (Singleton) 
// captures a shorter-lived service (Scoped/Transient).
// The captured service will live as long as the capturing service,
// which can cause issues like stale data, memory leaks, or thread-safety problems.
// ============================================================================

#region Captive Dependency Problem

public static class CaptiveDependencyProblem
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n--- CAPTIVE DEPENDENCY PROBLEM ---\n");

        var services = new ServiceCollection();

        // IScopedService is registered as SCOPED (new instance per scope)
        services.AddScoped<IScopedService, ScopedService>();

        // ISingletonConsumer is registered as SINGLETON (one instance for entire app lifetime)
        // PROBLEM: It captures IScopedService, making it live forever!
        services.AddSingleton<ISingletonConsumer, SingletonConsumer>();

        var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            // Enable validation to catch captive dependencies at build time
            ValidateScopes = true,
            ValidateOnBuild = true
        });

        try
        {
            // This will throw an exception because of captive dependency!
            using var scope = provider.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<ISingletonConsumer>();
            consumer.DoWork();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"❌ EXCEPTION CAUGHT: {ex.Message}");
            Console.WriteLine("\n💡 The captive dependency was detected!");
        }

        // Without validation, this is what happens:
        Console.WriteLine("\n--- Without Scope Validation (DANGEROUS!) ---");
        var unsafeProvider = services.BuildServiceProvider(); // No validation

        Console.WriteLine("Creating first scope...");
        using (var scope1 = unsafeProvider.CreateScope())
        {
            var consumer1 = scope1.ServiceProvider.GetRequiredService<ISingletonConsumer>();
            consumer1.DoWork();
        }

        Console.WriteLine("\nCreating second scope...");
        using (var scope2 = unsafeProvider.CreateScope())
        {
            var consumer2 = scope2.ServiceProvider.GetRequiredService<ISingletonConsumer>();
            consumer2.DoWork();
            // Notice: The same ScopedService instance is used!
            // This is the captive dependency in action - the scoped service
            // is now living as long as the singleton.
        }

        await Task.CompletedTask;
    }
}

// Simulates a scoped service (e.g., DbContext, HttpClient with user context)
public interface IScopedService
{
    Guid InstanceId { get; }
    void Execute();
}

public class ScopedService : IScopedService
{
    public Guid InstanceId { get; } = Guid.NewGuid();

    public ScopedService()
    {
        Console.WriteLine($"  🆕 ScopedService created: {InstanceId}");
    }

    public void Execute()
    {
        Console.WriteLine($"  📋 ScopedService executing: {InstanceId}");
    }
}

// Singleton that INCORRECTLY captures a scoped dependency
public interface ISingletonConsumer
{
    void DoWork();
}

public class SingletonConsumer : ISingletonConsumer
{
    private readonly IScopedService _scopedService; // CAPTIVE!

    public SingletonConsumer(IScopedService scopedService)
    {
        _scopedService = scopedService;
        Console.WriteLine($"  🔒 SingletonConsumer created, captured ScopedService: {_scopedService.InstanceId}");
    }

    public void DoWork()
    {
        Console.WriteLine($"  ⚙️ SingletonConsumer.DoWork() using ScopedService: {_scopedService.InstanceId}");
        _scopedService.Execute();
    }
}

#endregion

#region Captive Dependency Solution

public static class CaptiveDependencySolution
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n--- CAPTIVE DEPENDENCY SOLUTION ---\n");

        var services = new ServiceCollection();

        services.AddScoped<IScopedService, ScopedService>();

        // SOLUTION 1: Use IServiceScopeFactory to create scopes on demand
        services.AddSingleton<ISingletonConsumerFixed, SingletonConsumerFixed>();

        // SOLUTION 2: Use a Func<T> factory
        services.AddSingleton<ISingletonConsumerWithFactory, SingletonConsumerWithFactory>();
        services.AddScoped<Func<IScopedService>>(sp => () => sp.GetRequiredService<IScopedService>());

        var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true
        });

        Console.WriteLine("=== SOLUTION 1: Using IServiceScopeFactory ===\n");

        using (var scope1 = provider.CreateScope())
        {
            var consumer = scope1.ServiceProvider.GetRequiredService<ISingletonConsumerFixed>();
            await consumer.DoWorkAsync();
        }

        Console.WriteLine();

        using (var scope2 = provider.CreateScope())
        {
            var consumer = scope2.ServiceProvider.GetRequiredService<ISingletonConsumerFixed>();
            await consumer.DoWorkAsync();
            // Now each call gets a fresh scoped service!
        }

        Console.WriteLine("\n=== SOLUTION 2: Using Func<T> Factory ===\n");

        using (var scope3 = provider.CreateScope())
        {
            var consumer = scope3.ServiceProvider.GetRequiredService<ISingletonConsumerWithFactory>();
            consumer.DoWork();
        }

        Console.WriteLine();

        using (var scope4 = provider.CreateScope())
        {
            var consumer = scope4.ServiceProvider.GetRequiredService<ISingletonConsumerWithFactory>();
            consumer.DoWork();
        }
    }
}

// SOLUTION 1: Use IServiceScopeFactory
public interface ISingletonConsumerFixed
{
    Task DoWorkAsync();
}

public class SingletonConsumerFixed : ISingletonConsumerFixed
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SingletonConsumerFixed(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        Console.WriteLine("  🔒 SingletonConsumerFixed created (no captive dependency!)");
    }

    public async Task DoWorkAsync()
    {
        // Create a new scope for each operation
        using var scope = _scopeFactory.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<IScopedService>();

        Console.WriteLine($"  ⚙️ SingletonConsumerFixed.DoWorkAsync() using fresh ScopedService: {scopedService.InstanceId}");
        scopedService.Execute();

        await Task.CompletedTask;
    }
}

// SOLUTION 2: Use Func<T> factory pattern
public interface ISingletonConsumerWithFactory
{
    void DoWork();
}

public class SingletonConsumerWithFactory : ISingletonConsumerWithFactory
{
    private readonly Func<IScopedService> _scopedServiceFactory;

    public SingletonConsumerWithFactory(Func<IScopedService> scopedServiceFactory)
    {
        _scopedServiceFactory = scopedServiceFactory;
        Console.WriteLine("  🔒 SingletonConsumerWithFactory created (using factory pattern!)");
    }

    public void DoWork()
    {
        // Get a fresh instance each time
        var scopedService = _scopedServiceFactory();
        Console.WriteLine($"  ⚙️ SingletonConsumerWithFactory.DoWork() using fresh ScopedService: {scopedService.InstanceId}");
        scopedService.Execute();
    }
}

#endregion


// ============================================================================
// PROBLEM 2: SCOPED SERVICE IN IHostedService
// ============================================================================
// IHostedService implementations are registered as SINGLETONS.
// You cannot inject scoped services directly into them because:
// 1. Hosted services live for the entire application lifetime
// 2. Scoped services are meant to live for a single "unit of work"
// 3. Direct injection would create a captive dependency
// ============================================================================

#region Scoped in HostedService Problem

public static class ScopedInHostedServiceProblem
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n--- SCOPED SERVICE IN IHOSTEDSERVICE PROBLEM ---\n");

        try
        {
            var host = Host.CreateDefaultBuilder()
                .UseDefaultServiceProvider((context,options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                    options.ValidateOnBuild = context.HostingEnvironment.IsDevelopment();
                })

                .ConfigureServices(services =>
                {
                    services.AddScoped<IScopedDatabaseService, ScopedDatabaseService>();

                    // PROBLEM: This will fail at runtime!
                    services.AddHostedService<ProblematicBackgroundService>();
                })
                .Build();

            // This will throw when the hosted service tries to start
            await host.StartAsync();
            await Task.Delay(2000);
            await host.StopAsync();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"❌ EXCEPTION: {ex.Message}");
            Console.WriteLine("\n💡 Cannot resolve scoped service from root provider!");
        }
    }
}

// Simulates a scoped database service (like EF Core DbContext)
public interface IScopedDatabaseService
{
    Guid InstanceId { get; }
    Task SaveDataAsync(string data);
}

public class ScopedDatabaseService : IScopedDatabaseService, IDisposable
{
    public Guid InstanceId { get; } = Guid.NewGuid();
    private bool _disposed;

    public ScopedDatabaseService()
    {
        Console.WriteLine($"  🆕 ScopedDatabaseService created: {InstanceId}");
    }

    public async Task SaveDataAsync(string data)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(ScopedDatabaseService));

        Console.WriteLine($"  💾 ScopedDatabaseService [{InstanceId}] saving: {data}");
        await Task.Delay(100); // Simulate DB operation
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Console.WriteLine($"  🗑️ ScopedDatabaseService disposed: {InstanceId}");
            _disposed = true;
        }
    }
}

// PROBLEMATIC: Directly injecting scoped service into hosted service
public class ProblematicBackgroundService : BackgroundService
{
    private readonly IScopedDatabaseService _dbService; // WRONG!

    public ProblematicBackgroundService(IScopedDatabaseService dbService)
    {
        _dbService = dbService;
        Console.WriteLine($"  ⚠️ ProblematicBackgroundService created with captive: {_dbService.InstanceId}");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // This would use a stale/captive database context!
            await _dbService.SaveDataAsync($"Data at {DateTime.Now}");
            //await Task.Delay(1000, stoppingToken);
        }
    }
}

#endregion

#region Scoped in HostedService Solution

public static class ScopedInHostedServiceSolution
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n--- SCOPED SERVICE IN IHOSTEDSERVICE SOLUTION ---\n");

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddScoped<IScopedDatabaseService, ScopedDatabaseService>();

                // SOLUTION: Use IServiceScopeFactory in the hosted service
                services.AddHostedService<CorrectBackgroundService>();
            })
            .Build();

        Console.WriteLine("Starting host...\n");
        await host.StartAsync();

        // Let it run for a few iterations
        await Task.Delay(3500);

        Console.WriteLine("\nStopping host...");
        await host.StopAsync();

        Console.WriteLine("\n✅ Host stopped gracefully!");
    }
}

// CORRECT: Using IServiceScopeFactory to create scopes
public class CorrectBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CorrectBackgroundService> _logger;

    public CorrectBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<CorrectBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        Console.WriteLine("  ✅ CorrectBackgroundService created with IServiceScopeFactory");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("  🚀 CorrectBackgroundService started\n");

        int iteration = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            iteration++;
            Console.WriteLine($"  --- Iteration {iteration} ---");

            try
            {
                // Create a new scope for each "unit of work"
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbService = scope.ServiceProvider
                        .GetRequiredService<IScopedDatabaseService>();

                    await dbService.SaveDataAsync($"Data from iteration {iteration}");

                    // The scoped service will be disposed when the scope ends
                }
                // ScopedDatabaseService is disposed here ☝️

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in background service");
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}

#endregion


// ============================================================================
// BONUS: Alternative Solutions for .NET 8+
// ============================================================================

#region Modern Solutions

// For .NET 8+, you can also use the new IServiceScopeFactory.CreateAsyncScope()
public class ModernBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ModernBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // CreateAsyncScope is preferred for async disposable services
            await using var scope = _scopeFactory.CreateAsyncScope();

            var dbService = scope.ServiceProvider
                .GetRequiredService<IScopedDatabaseService>();

            await dbService.SaveDataAsync("Modern approach");

            await Task.Delay(1000, stoppingToken);
        }
    }
}

// Another pattern: Using a dedicated "processor" class
public interface IWorkProcessor
{
    Task ProcessAsync(CancellationToken cancellationToken);
}

public class WorkProcessor : IWorkProcessor
{
    private readonly IScopedDatabaseService _dbService;

    public WorkProcessor(IScopedDatabaseService dbService)
    {
        _dbService = dbService;
    }

    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        await _dbService.SaveDataAsync("Processed via dedicated class");
    }
}

// The hosted service resolves the processor in a scope
public class ProcessorBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ProcessorBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();

            // The processor and its scoped dependencies are created fresh
            var processor = scope.ServiceProvider.GetRequiredService<IWorkProcessor>();
            await processor.ProcessAsync(stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}

#endregion
