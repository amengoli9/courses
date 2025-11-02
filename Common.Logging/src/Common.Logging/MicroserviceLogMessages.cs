using Microsoft.Extensions.Logging;

namespace Common.Logging;

/// <summary>
/// Source-generated logging methods for microservice communication.
/// Uses LoggerMessage attribute for high-performance, zero-allocation logging.
/// </summary>
public static partial class MicroserviceLogMessages
{
    // Service Discovery

    [LoggerMessage(
        EventId = 4000,
        Level = LogLevel.Information,
        Message = "Discovering service: {ServiceName}")]
    public static partial void DiscoveringService(
        ILogger logger,
        string serviceName);

    [LoggerMessage(
        EventId = 4001,
        Level = LogLevel.Information,
        Message = "Service discovered: {ServiceName} - Endpoint: {Endpoint}")]
    public static partial void ServiceDiscovered(
        ILogger logger,
        string serviceName,
        string endpoint);

    [LoggerMessage(
        EventId = 4002,
        Level = LogLevel.Error,
        Message = "Service discovery failed: {ServiceName}")]
    public static partial void ServiceDiscoveryFailed(
        ILogger logger,
        Exception exception,
        string serviceName);

    // Inter-Service Communication

    [LoggerMessage(
        EventId = 4100,
        Level = LogLevel.Information,
        Message = "Calling microservice: {TargetService} - Operation: {Operation} - TraceId: {TraceId}")]
    public static partial void CallingMicroservice(
        ILogger logger,
        string targetService,
        string operation,
        string traceId);

    [LoggerMessage(
        EventId = 4101,
        Level = LogLevel.Information,
        Message = "Microservice call completed: {TargetService} - Operation: {Operation} - Status: {StatusCode} - Duration: {DurationMs}ms")]
    public static partial void MicroserviceCallCompleted(
        ILogger logger,
        string targetService,
        string operation,
        int statusCode,
        long durationMs);

    [LoggerMessage(
        EventId = 4102,
        Level = LogLevel.Error,
        Message = "Microservice call failed: {TargetService} - Operation: {Operation} - Error: {ErrorMessage}")]
    public static partial void MicroserviceCallFailed(
        ILogger logger,
        Exception exception,
        string targetService,
        string operation,
        string errorMessage);

    [LoggerMessage(
        EventId = 4103,
        Level = LogLevel.Warning,
        Message = "Microservice unavailable: {TargetService} - Will retry in {RetryDelayMs}ms - Attempt: {AttemptNumber}")]
    public static partial void MicroserviceUnavailable(
        ILogger logger,
        string targetService,
        int retryDelayMs,
        int attemptNumber);

    // Circuit Breaker

    [LoggerMessage(
        EventId = 4200,
        Level = LogLevel.Warning,
        Message = "Circuit breaker opened for service: {ServiceName} - Failure count: {FailureCount}")]
    public static partial void CircuitBreakerOpened(
        ILogger logger,
        string serviceName,
        int failureCount);

    [LoggerMessage(
        EventId = 4201,
        Level = LogLevel.Information,
        Message = "Circuit breaker half-opened for service: {ServiceName} - Testing connection")]
    public static partial void CircuitBreakerHalfOpened(
        ILogger logger,
        string serviceName);

    [LoggerMessage(
        EventId = 4202,
        Level = LogLevel.Information,
        Message = "Circuit breaker closed for service: {ServiceName} - Service recovered")]
    public static partial void CircuitBreakerClosed(
        ILogger logger,
        string serviceName);

    [LoggerMessage(
        EventId = 4203,
        Level = LogLevel.Warning,
        Message = "Circuit breaker rejected call to service: {ServiceName} - State: {CircuitState}")]
    public static partial void CircuitBreakerRejectedCall(
        ILogger logger,
        string serviceName,
        string circuitState);

    // Message Queue

    [LoggerMessage(
        EventId = 4300,
        Level = LogLevel.Information,
        Message = "Publishing message: {MessageType} to queue: {QueueName} - MessageId: {MessageId}")]
    public static partial void PublishingMessage(
        ILogger logger,
        string messageType,
        string queueName,
        string messageId);

    [LoggerMessage(
        EventId = 4301,
        Level = LogLevel.Information,
        Message = "Message published successfully: {MessageType} - MessageId: {MessageId}")]
    public static partial void MessagePublished(
        ILogger logger,
        string messageType,
        string messageId);

    [LoggerMessage(
        EventId = 4302,
        Level = LogLevel.Information,
        Message = "Processing message: {MessageType} - MessageId: {MessageId} - Attempt: {AttemptNumber}")]
    public static partial void ProcessingMessage(
        ILogger logger,
        string messageType,
        string messageId,
        int attemptNumber);

    [LoggerMessage(
        EventId = 4303,
        Level = LogLevel.Information,
        Message = "Message processed successfully: {MessageType} - MessageId: {MessageId} - Duration: {DurationMs}ms")]
    public static partial void MessageProcessed(
        ILogger logger,
        string messageType,
        string messageId,
        long durationMs);

    [LoggerMessage(
        EventId = 4304,
        Level = LogLevel.Error,
        Message = "Message processing failed: {MessageType} - MessageId: {MessageId}")]
    public static partial void MessageProcessingFailed(
        ILogger logger,
        Exception exception,
        string messageType,
        string messageId);

    [LoggerMessage(
        EventId = 4305,
        Level = LogLevel.Warning,
        Message = "Message sent to dead letter queue: {MessageType} - MessageId: {MessageId} - Reason: {Reason}")]
    public static partial void MessageDeadLettered(
        ILogger logger,
        string messageType,
        string messageId,
        string reason);

    // Distributed Tracing

    [LoggerMessage(
        EventId = 4400,
        Level = LogLevel.Debug,
        Message = "Trace context propagated: TraceId: {TraceId} - SpanId: {SpanId} - ParentSpanId: {ParentSpanId}")]
    public static partial void TraceContextPropagated(
        ILogger logger,
        string traceId,
        string spanId,
        string? parentSpanId);

    [LoggerMessage(
        EventId = 4401,
        Level = LogLevel.Debug,
        Message = "Creating child span: {SpanName} - TraceId: {TraceId}")]
    public static partial void CreatingChildSpan(
        ILogger logger,
        string spanName,
        string traceId);
}
