using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DatabaseContext;

/// <summary>
/// Real-World Example: Multiple Database Contexts (Read/Write Separation, Sharding)
///
/// Demonstrates using KeyedServices for:
/// - Read/Write database separation (CQRS pattern)
/// - Multi-tenant databases
/// - Database sharding
/// </summary>

// ========================================
// DATABASE CONNECTION ABSTRACTION
// ========================================

public interface IDatabaseConnection
{
    string ConnectionName { get; }
    Task<T> QueryAsync<T>(string sql) where T : class, new();
    Task<int> ExecuteAsync(string sql);
}

public class ReadOnlyDatabaseConnection : IDatabaseConnection
{
    public string ConnectionName => "ReadOnly (Replica)";

    public async Task<T> QueryAsync<T>(string sql) where T : class, new()
    {
        Console.WriteLine($"📖 [READ DB] Executing: {sql}");
        await Task.Delay(50); // Simulate query
        Console.WriteLine($"   ✓ Query completed from read replica");
        return new T();
    }

    public async Task<int> ExecuteAsync(string sql)
    {
        throw new InvalidOperationException("Cannot execute write operations on read-only database");
    }
}

public class WriteDatabase Connection : IDatabaseConnection
{
    public string ConnectionName => "Primary (Write)";

    public async Task<T> QueryAsync<T>(string sql) where T : class, new()
    {
        Console.WriteLine($"📝 [WRITE DB] Executing: {sql}");
        await Task.Delay(75); // Simulate query
        Console.WriteLine($"   ✓ Query completed from primary database");
        return new T();
    }

    public async Task<int> ExecuteAsync(string sql)
    {
        Console.WriteLine($"✍️ [WRITE DB] Executing: {sql}");
        await Task.Delay(100); // Simulate write
        Console.WriteLine($"   ✓ Write operation completed");
        return 1;
    }
}

public class AnalyticsDatabaseConnection : IDatabaseConnection
{
    public string ConnectionName => "Analytics (Warehouse)";

    public async Task<T> QueryAsync<T>(string sql) where T : class, new()
    {
        Console.WriteLine($"📊 [ANALYTICS DB] Executing: {sql}");
        await Task.Delay(200); // Analytics queries are slower
        Console.WriteLine($"   ✓ Analytics query completed");
        return new T();
    }

    public async Task<int> ExecuteAsync(string sql)
    {
        throw new InvalidOperationException("Cannot execute write operations on analytics database");
    }
}

// ========================================
// REPOSITORY USING READ/WRITE SEPARATION
// ========================================

public class UserRepository
{
    private readonly IDatabaseConnection _readDb;
    private readonly IDatabaseConnection _writeDb;

    public UserRepository(
        [FromKeyedServices("read")] IDatabaseConnection readDb,
        [FromKeyedServices("write")] IDatabaseConnection writeDb)
    {
        _readDb = readDb;
        _writeDb = writeDb;
    }

    public async Task<object> GetUserByIdAsync(int userId)
    {
        Console.WriteLine($"\n👤 Getting user {userId} (using READ database):");
        return await _readDb.QueryAsync<object>($"SELECT * FROM Users WHERE Id = {userId}");
    }

    public async Task<int> CreateUserAsync(string username, string email)
    {
        Console.WriteLine($"\n👤 Creating user '{username}' (using WRITE database):");
        return await _writeDb.ExecuteAsync($"INSERT INTO Users (Username, Email) VALUES ('{username}', '{email}')");
    }

    public async Task<int> UpdateUserAsync(int userId, string email)
    {
        Console.WriteLine($"\n👤 Updating user {userId} (using WRITE database):");
        return await _writeDb.ExecuteAsync($"UPDATE Users SET Email = '{email}' WHERE Id = {userId}");
    }
}

// ========================================
// ANALYTICS SERVICE
// ========================================

public class AnalyticsService
{
    private readonly IDatabaseConnection _analyticsDb;

    public AnalyticsService([FromKeyedServices("analytics")] IDatabaseConnection analyticsDb)
    {
        _analyticsDb = analyticsDb;
    }

    public async Task<object> GenerateUserReportAsync()
    {
        Console.WriteLine($"\n📊 Generating user analytics report:");
        return await _analyticsDb.QueryAsync<object>(
            "SELECT COUNT(*), AVG(age), Country FROM Users GROUP BY Country");
    }

    public async Task<object> GenerateSalesReportAsync()
    {
        Console.WriteLine($"\n📊 Generating sales analytics:");
        return await _analyticsDb.QueryAsync<object>(
            "SELECT SUM(amount), DATE_TRUNC('day', created_at) FROM Orders GROUP BY 2");
    }
}

// ========================================
// MULTI-TENANT DATABASE SERVICE
// ========================================

public class MultiTenantService
{
    private readonly IServiceProvider _serviceProvider;

    public MultiTenantService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<object> QueryTenantDataAsync(string tenantId, string sql)
    {
        Console.WriteLine($"\n🏢 Querying data for tenant: {tenantId}");

        var dbConnection = _serviceProvider.GetRequiredKeyedService<IDatabaseConnection>($"tenant-{tenantId}");
        return await dbConnection.QueryAsync<object>(sql);
    }
}

// ========================================
// TENANT-SPECIFIC DATABASE CONNECTIONS
// ========================================

public class TenantADatabaseConnection : IDatabaseConnection
{
    public string ConnectionName => "Tenant A Database";

    public async Task<T> QueryAsync<T>(string sql) where T : class, new()
    {
        Console.WriteLine($"🏢 [TENANT-A DB] {sql}");
        await Task.Delay(60);
        return new T();
    }

    public async Task<int> ExecuteAsync(string sql)
    {
        Console.WriteLine($"🏢 [TENANT-A DB] {sql}");
        await Task.Delay(80);
        return 1;
    }
}

public class TenantBDatabaseConnection : IDatabaseConnection
{
    public string ConnectionName => "Tenant B Database";

    public async Task<T> QueryAsync<T>(string sql) where T : class, new()
    {
        Console.WriteLine($"🏢 [TENANT-B DB] {sql}");
        await Task.Delay(60);
        return new T();
    }

    public async Task<int> ExecuteAsync(string sql)
    {
        Console.WriteLine($"🏢 [TENANT-B DB] {sql}");
        await Task.Delay(80);
        return 1;
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
        Console.WriteLine("KEYED SERVICES: DATABASE CONTEXT EXAMPLES");
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine();

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Read/Write Separation (CQRS)
                services.AddKeyedSingleton<IDatabaseConnection, ReadOnlyDatabaseConnection>("read");
                services.AddKeyedSingleton<IDatabaseConnection, WriteDatabaseConnection>("write");
                services.AddKeyedSingleton<IDatabaseConnection, AnalyticsDatabaseConnection>("analytics");

                // Multi-tenant databases
                services.AddKeyedSingleton<IDatabaseConnection, TenantADatabaseConnection>("tenant-A");
                services.AddKeyedSingleton<IDatabaseConnection, TenantBDatabaseConnection>("tenant-B");

                // Services
                services.AddSingleton<UserRepository>();
                services.AddSingleton<AnalyticsService>();
                services.AddSingleton<MultiTenantService>();
            })
            .Build();

        // ========================================
        // DEMO 1: Read/Write Separation
        // ========================================
        Console.WriteLine("DEMO 1: Read/Write Database Separation (CQRS)");
        Console.WriteLine("-" .PadRight(70, '-'));

        var userRepo = host.Services.GetRequiredService<UserRepository>();

        await userRepo.GetUserByIdAsync(123);
        await userRepo.CreateUserAsync("john.doe", "john@example.com");
        await userRepo.UpdateUserAsync(123, "john.doe@newdomain.com");

        // ========================================
        // DEMO 2: Analytics Database
        // ========================================
        Console.WriteLine("\n\nDEMO 2: Separate Analytics Database");
        Console.WriteLine("-" .PadRight(70, '-'));

        var analyticsService = host.Services.GetRequiredService<AnalyticsService>();
        await analyticsService.GenerateUserReportAsync();
        await analyticsService.GenerateSalesReportAsync();

        // ========================================
        // DEMO 3: Multi-Tenant Databases
        // ========================================
        Console.WriteLine("\n\nDEMO 3: Multi-Tenant Database Isolation");
        Console.WriteLine("-" .PadRight(70, '-'));

        var multiTenantService = host.Services.GetRequiredService<MultiTenantService>();
        await multiTenantService.QueryTenantDataAsync("A", "SELECT * FROM Orders");
        await multiTenantService.QueryTenantDataAsync("B", "SELECT * FROM Orders");

        // ========================================
        // SUMMARY
        // ========================================
        Console.WriteLine("\n\n" + "=" .PadRight(70, '='));
        Console.WriteLine("BENEFITS:");
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine("✓ Read/Write separation for performance");
        Console.WriteLine("✓ Dedicated analytics database");
        Console.WriteLine("✓ Multi-tenant database isolation");
        Console.WriteLine("✓ Easy to scale with database sharding");
        Console.WriteLine("✓ Clear separation of concerns");
        Console.WriteLine();
    }
}
