using Microsoft.Extensions.Logging;

namespace Common.Logging;

/// <summary>
/// Source-generated logger per operazioni HTTP
/// Usa ILogger con metodi partial per zero-allocation logging
/// </summary>
public static partial class Log
{
    // HTTP Client Operations (1000-1099)

    [LoggerMessage(
        EventId = 1000,
        Level = LogLevel.Information,
        Message = "Calling microservice {TargetService} - Operation: {Operation} - TraceId: {TraceId}")]
    public static partial void CallingMicroservice(
        this ILogger logger,
        string targetService,
        string operation,
        string traceId);

    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Microservice call completed: {TargetService} - Operation: {Operation} - Status: {StatusCode} - Duration: {DurationMs}ms")]
    public static partial void MicroserviceCallCompleted(
        this ILogger logger,
        string targetService,
        string operation,
        int statusCode,
        long durationMs);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Error,
        Message = "Microservice call failed: {TargetService} - Operation: {Operation} - Error: {ErrorMessage}")]
    public static partial void MicroserviceCallFailed(
        this ILogger logger,
        Exception exception,
        string targetService,
        string operation,
        string errorMessage);

    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Warning,
        Message = "Microservice unavailable: {TargetService} - Will retry - Attempt: {AttemptNumber}")]
    public static partial void MicroserviceUnavailable(
        this ILogger logger,
        string targetService,
        int attemptNumber);

    [LoggerMessage(
        EventId = 1100,
        Level = LogLevel.Information,
        Message = "HTTP {Method} {Path} - Request started")]
    public static partial void HttpRequestStarted(
        this ILogger logger,
        string method,
        string path);

    [LoggerMessage(
        EventId = 1101,
        Level = LogLevel.Information,
        Message = "HTTP {Method} {Path} completed - Status: {StatusCode} - Duration: {DurationMs}ms")]
    public static partial void HttpRequestCompleted(
        this ILogger logger,
        string method,
        string path,
        int statusCode,
        long durationMs);

    // Business Operations (3000-3099)

    [LoggerMessage(
        EventId = 3000,
        Level = LogLevel.Information,
        Message = "Creating {EntityType} with ID: {EntityId}")]
    public static partial void EntityCreating(
        this ILogger logger,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3001,
        Level = LogLevel.Information,
        Message = "{EntityType} created successfully: {EntityId}")]
    public static partial void EntityCreated(
        this ILogger logger,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3002,
        Level = LogLevel.Information,
        Message = "Updating {EntityType} with ID: {EntityId}")]
    public static partial void EntityUpdating(
        this ILogger logger,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3003,
        Level = LogLevel.Information,
        Message = "{EntityType} updated successfully: {EntityId}")]
    public static partial void EntityUpdated(
        this ILogger logger,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3004,
        Level = LogLevel.Information,
        Message = "Deleting {EntityType} with ID: {EntityId}")]
    public static partial void EntityDeleting(
        this ILogger logger,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3005,
        Level = LogLevel.Information,
        Message = "{EntityType} deleted successfully: {EntityId}")]
    public static partial void EntityDeleted(
        this ILogger logger,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3006,
        Level = LogLevel.Warning,
        Message = "{EntityType} not found: {EntityId}")]
    public static partial void EntityNotFound(
        this ILogger logger,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3100,
        Level = LogLevel.Information,
        Message = "Service operation started: {ServiceName}.{OperationName}")]
    public static partial void ServiceOperationStarted(
        this ILogger logger,
        string serviceName,
        string operationName);

    [LoggerMessage(
        EventId = 3101,
        Level = LogLevel.Information,
        Message = "Service operation completed: {ServiceName}.{OperationName} - Duration: {DurationMs}ms")]
    public static partial void ServiceOperationCompleted(
        this ILogger logger,
        string serviceName,
        string operationName,
        long durationMs);

    [LoggerMessage(
        EventId = 3102,
        Level = LogLevel.Error,
        Message = "Service operation failed: {ServiceName}.{OperationName}")]
    public static partial void ServiceOperationFailed(
        this ILogger logger,
        Exception exception,
        string serviceName,
        string operationName);

    [LoggerMessage(
        EventId = 3103,
        Level = LogLevel.Warning,
        Message = "Business rule violation: {RuleName} - {EntityType} {EntityId} - Reason: {Reason}")]
    public static partial void BusinessRuleViolation(
        this ILogger logger,
        string ruleName,
        string entityType,
        string entityId,
        string reason);

    // Database Operations (2000-2099)

    [LoggerMessage(
        EventId = 2000,
        Level = LogLevel.Debug,
        Message = "Executing query: {QueryName}")]
    public static partial void ExecutingQuery(
        this ILogger logger,
        string queryName);

    [LoggerMessage(
        EventId = 2001,
        Level = LogLevel.Debug,
        Message = "Query completed: {QueryName} - Rows: {RowCount} - Duration: {DurationMs}ms")]
    public static partial void QueryCompleted(
        this ILogger logger,
        string queryName,
        int rowCount,
        long durationMs);

    [LoggerMessage(
        EventId = 2002,
        Level = LogLevel.Error,
        Message = "Query failed: {QueryName}")]
    public static partial void QueryFailed(
        this ILogger logger,
        Exception exception,
        string queryName);

    [LoggerMessage(
        EventId = 2003,
        Level = LogLevel.Warning,
        Message = "Slow query detected: {QueryName} - Duration: {DurationMs}ms")]
    public static partial void SlowQueryDetected(
        this ILogger logger,
        string queryName,
        long durationMs);
}
