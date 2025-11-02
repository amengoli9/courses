using Microsoft.Extensions.Logging;

namespace Common.Logging;

/// <summary>
/// Source-generated logging methods for business logic operations.
/// Uses LoggerMessage attribute for high-performance, zero-allocation logging.
/// </summary>
public static partial class BusinessLogMessages
{
    // Entity Operations

    [LoggerMessage(
        EventId = 3000,
        Level = LogLevel.Information,
        Message = "Creating {EntityType} with ID: {EntityId}")]
    public static partial void CreatingEntity(
        ILogger logger,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3001,
        Level = LogLevel.Information,
        Message = "{EntityType} created successfully: {EntityId}")]
    public static partial void EntityCreated(
        ILogger logger,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3002,
        Level = LogLevel.Information,
        Message = "Updating {EntityType} with ID: {EntityId}")]
    public static partial void UpdatingEntity(
        ILogger logger,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3003,
        Level = LogLevel.Information,
        Message = "{EntityType} updated successfully: {EntityId}")]
    public static partial void EntityUpdated(
        ILogger logger,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3004,
        Level = LogLevel.Information,
        Message = "Deleting {EntityType} with ID: {EntityId}")]
    public static partial void DeletingEntity(
        ILogger logger,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3005,
        Level = LogLevel.Information,
        Message = "{EntityType} deleted successfully: {EntityId}")]
    public static partial void EntityDeleted(
        ILogger logger,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3006,
        Level = LogLevel.Warning,
        Message = "{EntityType} not found: {EntityId}")]
    public static partial void EntityNotFound(
        ILogger logger,
        string entityType,
        string entityId);

    // Business Rule Validation

    [LoggerMessage(
        EventId = 3100,
        Level = LogLevel.Warning,
        Message = "Business rule violation: {RuleName} - {EntityType} {EntityId} - Reason: {Reason}")]
    public static partial void BusinessRuleViolation(
        ILogger logger,
        string ruleName,
        string entityType,
        string entityId,
        string reason);

    [LoggerMessage(
        EventId = 3101,
        Level = LogLevel.Information,
        Message = "Business rule validation passed: {RuleName} - {EntityType} {EntityId}")]
    public static partial void BusinessRuleValidationPassed(
        ILogger logger,
        string ruleName,
        string entityType,
        string entityId);

    [LoggerMessage(
        EventId = 3102,
        Level = LogLevel.Error,
        Message = "Business operation failed: {OperationName} - {EntityType} {EntityId}")]
    public static partial void BusinessOperationFailed(
        ILogger logger,
        Exception exception,
        string operationName,
        string entityType,
        string entityId);

    // Domain Events

    [LoggerMessage(
        EventId = 3200,
        Level = LogLevel.Information,
        Message = "Domain event raised: {EventType} - Aggregate: {AggregateId}")]
    public static partial void DomainEventRaised(
        ILogger logger,
        string eventType,
        string aggregateId);

    [LoggerMessage(
        EventId = 3201,
        Level = LogLevel.Information,
        Message = "Domain event handled: {EventType} - Handler: {HandlerName} - Duration: {DurationMs}ms")]
    public static partial void DomainEventHandled(
        ILogger logger,
        string eventType,
        string handlerName,
        long durationMs);

    [LoggerMessage(
        EventId = 3202,
        Level = LogLevel.Error,
        Message = "Domain event handler failed: {EventType} - Handler: {HandlerName}")]
    public static partial void DomainEventHandlerFailed(
        ILogger logger,
        Exception exception,
        string eventType,
        string handlerName);

    // Service Operations

    [LoggerMessage(
        EventId = 3300,
        Level = LogLevel.Information,
        Message = "Service operation started: {ServiceName}.{OperationName}")]
    public static partial void ServiceOperationStarted(
        ILogger logger,
        string serviceName,
        string operationName);

    [LoggerMessage(
        EventId = 3301,
        Level = LogLevel.Information,
        Message = "Service operation completed: {ServiceName}.{OperationName} - Duration: {DurationMs}ms")]
    public static partial void ServiceOperationCompleted(
        ILogger logger,
        string serviceName,
        string operationName,
        long durationMs);

    [LoggerMessage(
        EventId = 3302,
        Level = LogLevel.Error,
        Message = "Service operation failed: {ServiceName}.{OperationName}")]
    public static partial void ServiceOperationFailed(
        ILogger logger,
        Exception exception,
        string serviceName,
        string operationName);

    [LoggerMessage(
        EventId = 3303,
        Level = LogLevel.Warning,
        Message = "Service operation cancelled: {ServiceName}.{OperationName} - Reason: {Reason}")]
    public static partial void ServiceOperationCancelled(
        ILogger logger,
        string serviceName,
        string operationName,
        string reason);
}
