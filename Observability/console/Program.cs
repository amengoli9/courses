using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ConsoleApp;

class Program
{
    // Define ActivitySource for custom tracing
    private static readonly ActivitySource ActivitySource = new("ConsoleApp", "1.0.0");

    // Define Meter for custom metrics
    private static readonly Meter Meter = new("ConsoleApp", "1.0.0");

    // Custom metrics
    private static readonly Counter<long> RequestCounter = Meter.CreateCounter<long>(
        "app.requests",
        description: "Total number of requests processed");

    private static readonly Histogram<double> RequestDuration = Meter.CreateHistogram<double>(
        "app.request.duration",
        unit: "ms",
        description: "Duration of requests in milliseconds");


   static async Task Main(string[] args)
    {
        Console.WriteLine("=== Console Application with OpenTelemetry ===\n");

        // Configure OpenTelemetry
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(serviceName: "ConsoleApp", serviceVersion: "1.0.0"))
            .AddSource(ActivitySource.Name)
            .AddHttpClientInstrumentation()
            .AddOtlpExporter()
            .Build();

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(serviceName: "ConsoleApp", serviceVersion: "1.0.0"))
            .AddMeter(Meter.Name)
            .AddRuntimeInstrumentation()
            .AddOtlpExporter()
            .Build();
      using var loggerFactory = LoggerFactory.Create(builder =>
      {
         // Add OpenTelemetry as a logging provider
         builder.AddOpenTelemetry(options =>
         {
            options.SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(serviceName: "ConsoleApp", serviceVersion: "1.0.0"));
            options.AddOtlpExporter();
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
         });
         builder.SetMinimumLevel(LogLevel.Information);
      });
      ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
      logger.LogInformation("Logger initialized with OpenTelemetry");

      logger.LogInformation("OpenTelemetry instrumentation configured successfully!\n");
        Console.WriteLine("Starting application workflow...\n");

        // Simulate application workflow with tracing
        await RunApplicationWorkflow(logger);

        Console.WriteLine("\nApplication completed. Check the traces and metrics above!");
    }

    private static async Task RunApplicationWorkflow(ILogger<Program> logger)
    {
        // Create a root activity/span
        using var activity = ActivitySource.StartActivity("ApplicationWorkflow", ActivityKind.Internal);
        activity?.SetTag("workflow.type", "demo");
        activity?.SetTag("workflow.version", "1.0");

        try
        {
            logger.LogInformation("Step 1: Processing user request...");
            await ProcessUserRequest("user123");

            logger.LogInformation("Step 2: Performing calculation...");
            await PerformCalculation(42);

            logger.LogInformation("Step 3: Simulating HTTP call...");
            await SimulateHttpCall();

            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.RecordException(ex);
            throw;
        }
    }

    private static async Task ProcessUserRequest(string userId)
    {
        using var activity = ActivitySource.StartActivity("ProcessUserRequest", ActivityKind.Internal);
        activity?.SetTag("user.id", userId);

        var sw = Stopwatch.StartNew();

        try
        {
            // Simulate some work
            await Task.Delay(Random.Shared.Next(50, 150));

            // Record metrics
            RequestCounter.Add(1, new KeyValuePair<string, object?>("operation", "process_request"));
            RequestDuration.Record(sw.Elapsed.TotalMilliseconds,
                new KeyValuePair<string, object?>("operation", "process_request"));

            activity?.AddEvent(new ActivityEvent("UserRequestProcessed",
                tags: new ActivityTagsCollection
                {
                    { "user.id", userId },
                    { "processing.time", sw.Elapsed.TotalMilliseconds }
                }));

            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.RecordException(ex);
            throw;
        }
    }

    private static async Task PerformCalculation(int input)
    {
        using var activity = ActivitySource.StartActivity("PerformCalculation", ActivityKind.Internal);
        activity?.SetTag("input.value", input);

        var sw = Stopwatch.StartNew();

        try
        {
            // Simulate calculation
            await Task.Delay(Random.Shared.Next(30, 100));
            var result = input * 2 + 10;

            activity?.SetTag("output.value", result);

            RequestCounter.Add(1, new KeyValuePair<string, object?>("operation", "calculation"));
            RequestDuration.Record(sw.Elapsed.TotalMilliseconds,
                new KeyValuePair<string, object?>("operation", "calculation"));

            activity?.AddEvent(new ActivityEvent("CalculationCompleted",
                tags: new ActivityTagsCollection
                {
                    { "result", result },
                    { "duration", sw.Elapsed.TotalMilliseconds }
                }));

            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.RecordException(ex);
            throw;
        }
    }

    private static async Task SimulateHttpCall()
    {
        using var activity = ActivitySource.StartActivity("SimulateHttpCall", ActivityKind.Client);
        activity?.SetTag("http.method", "GET");
        activity?.SetTag("http.url", "https://api.example.com/data");

        var sw = Stopwatch.StartNew();

        try
        {
            // Simulate HTTP call
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "ConsoleApp/1.0");

            // Simulate delay instead of actual HTTP call
            await Task.Delay(Random.Shared.Next(100, 300));

            activity?.SetTag("http.status_code", 200);

            RequestCounter.Add(1, new KeyValuePair<string, object?>("operation", "http_call"));
            RequestDuration.Record(sw.Elapsed.TotalMilliseconds,
                new KeyValuePair<string, object?>("operation", "http_call"));

            activity?.AddEvent(new ActivityEvent("HttpCallCompleted",
                tags: new ActivityTagsCollection
                {
                    { "http.status_code", 200 },
                    { "duration", sw.Elapsed.TotalMilliseconds }
                }));

            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception ex)
        {
            activity?.SetTag("http.status_code", 500);
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.RecordException(ex);
            throw;
        }
    }
}
