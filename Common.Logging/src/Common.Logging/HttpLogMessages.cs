using Microsoft.Extensions.Logging;

namespace Common.Logging;

/// <summary>
/// Source-generated logging methods for HTTP operations.
/// Uses LoggerMessage attribute for high-performance, zero-allocation logging.
/// </summary>
public static partial class HttpLogMessages
{
    // HTTP Client Calls

    [LoggerMessage(
        EventId = 1000,
        Level = LogLevel.Information,
        Message = "Calling external API: {Method} {Url}")]
    public static partial void CallingExternalApi(
        ILogger logger,
        string method,
        string url);

    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "External API call completed: {Method} {Url} - Status: {StatusCode} - Duration: {DurationMs}ms")]
    public static partial void ExternalApiCallCompleted(
        ILogger logger,
        string method,
        string url,
        int statusCode,
        long durationMs);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Error,
        Message = "External API call failed: {Method} {Url} - Error: {ErrorMessage}")]
    public static partial void ExternalApiCallFailed(
        ILogger logger,
        Exception exception,
        string method,
        string url,
        string errorMessage);

    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Warning,
        Message = "External API call timed out: {Method} {Url} - Timeout: {TimeoutSeconds}s")]
    public static partial void ExternalApiCallTimedOut(
        ILogger logger,
        string method,
        string url,
        int timeoutSeconds);

    // HTTP Server Requests

    [LoggerMessage(
        EventId = 1100,
        Level = LogLevel.Information,
        Message = "HTTP {Method} {Path} - Request started")]
    public static partial void RequestStarted(
        ILogger logger,
        string method,
        string path);

    [LoggerMessage(
        EventId = 1101,
        Level = LogLevel.Information,
        Message = "HTTP {Method} {Path} - Request completed - Status: {StatusCode} - Duration: {DurationMs}ms")]
    public static partial void RequestCompleted(
        ILogger logger,
        string method,
        string path,
        int statusCode,
        long durationMs);

    [LoggerMessage(
        EventId = 1102,
        Level = LogLevel.Error,
        Message = "HTTP {Method} {Path} - Unhandled exception")]
    public static partial void UnhandledException(
        ILogger logger,
        Exception exception,
        string method,
        string path);

    [LoggerMessage(
        EventId = 1103,
        Level = LogLevel.Warning,
        Message = "HTTP {Method} {Path} - Validation failed: {ValidationErrors}")]
    public static partial void ValidationFailed(
        ILogger logger,
        string method,
        string path,
        string validationErrors);
}
