using Microsoft.Extensions.Logging;

namespace Common.Logging;

/// <summary>
/// Source-generated logging methods for database operations.
/// Uses LoggerMessage attribute for high-performance, zero-allocation logging.
/// </summary>
public static partial class DatabaseLogMessages
{
    // Query Operations

    [LoggerMessage(
        EventId = 2000,
        Level = LogLevel.Debug,
        Message = "Executing query: {QueryName} - Parameters: {Parameters}")]
    public static partial void ExecutingQuery(
        ILogger logger,
        string queryName,
        string parameters);

    [LoggerMessage(
        EventId = 2001,
        Level = LogLevel.Debug,
        Message = "Query completed: {QueryName} - Rows affected: {RowsAffected} - Duration: {DurationMs}ms")]
    public static partial void QueryCompleted(
        ILogger logger,
        string queryName,
        int rowsAffected,
        long durationMs);

    [LoggerMessage(
        EventId = 2002,
        Level = LogLevel.Error,
        Message = "Query failed: {QueryName} - Error: {ErrorMessage}")]
    public static partial void QueryFailed(
        ILogger logger,
        Exception exception,
        string queryName,
        string errorMessage);

    [LoggerMessage(
        EventId = 2003,
        Level = LogLevel.Warning,
        Message = "Slow query detected: {QueryName} - Duration: {DurationMs}ms - Threshold: {ThresholdMs}ms")]
    public static partial void SlowQueryDetected(
        ILogger logger,
        string queryName,
        long durationMs,
        long thresholdMs);

    // Transaction Operations

    [LoggerMessage(
        EventId = 2100,
        Level = LogLevel.Information,
        Message = "Transaction started: {TransactionId}")]
    public static partial void TransactionStarted(
        ILogger logger,
        string transactionId);

    [LoggerMessage(
        EventId = 2101,
        Level = LogLevel.Information,
        Message = "Transaction committed: {TransactionId} - Duration: {DurationMs}ms")]
    public static partial void TransactionCommitted(
        ILogger logger,
        string transactionId,
        long durationMs);

    [LoggerMessage(
        EventId = 2102,
        Level = LogLevel.Warning,
        Message = "Transaction rolled back: {TransactionId} - Reason: {Reason}")]
    public static partial void TransactionRolledBack(
        ILogger logger,
        string transactionId,
        string reason);

    [LoggerMessage(
        EventId = 2103,
        Level = LogLevel.Error,
        Message = "Transaction failed: {TransactionId}")]
    public static partial void TransactionFailed(
        ILogger logger,
        Exception exception,
        string transactionId);

    // Connection Operations

    [LoggerMessage(
        EventId = 2200,
        Level = LogLevel.Debug,
        Message = "Opening database connection: {ConnectionString}")]
    public static partial void OpeningConnection(
        ILogger logger,
        string connectionString);

    [LoggerMessage(
        EventId = 2201,
        Level = LogLevel.Debug,
        Message = "Database connection opened: {ConnectionId} - Duration: {DurationMs}ms")]
    public static partial void ConnectionOpened(
        ILogger logger,
        string connectionId,
        long durationMs);

    [LoggerMessage(
        EventId = 2202,
        Level = LogLevel.Error,
        Message = "Failed to open database connection: {ConnectionString}")]
    public static partial void ConnectionFailed(
        ILogger logger,
        Exception exception,
        string connectionString);

    [LoggerMessage(
        EventId = 2203,
        Level = LogLevel.Debug,
        Message = "Database connection closed: {ConnectionId}")]
    public static partial void ConnectionClosed(
        ILogger logger,
        string connectionId);

    // Migration Operations

    [LoggerMessage(
        EventId = 2300,
        Level = LogLevel.Information,
        Message = "Applying database migration: {MigrationName}")]
    public static partial void ApplyingMigration(
        ILogger logger,
        string migrationName);

    [LoggerMessage(
        EventId = 2301,
        Level = LogLevel.Information,
        Message = "Database migration completed: {MigrationName} - Duration: {DurationMs}ms")]
    public static partial void MigrationCompleted(
        ILogger logger,
        string migrationName,
        long durationMs);

    [LoggerMessage(
        EventId = 2302,
        Level = LogLevel.Error,
        Message = "Database migration failed: {MigrationName}")]
    public static partial void MigrationFailed(
        ILogger logger,
        Exception exception,
        string migrationName);
}
