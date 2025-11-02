using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Restaurant.Domain.Repositories;
using Restaurant.Domain.Services;
using Restaurant.Infrastructure.Data;
using Restaurant.Infrastructure.Repositories;
using Serilog;
using Serilog.Events;
using System.Diagnostics;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/restaurant-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}",
        retainedFileCountLimit: 30)
    // Send logs to Seq (local observability platform)
    .WriteTo.Seq("http://localhost:5341")
    // Send logs to OpenTelemetry collector
    .WriteTo.OpenTelemetry(options =>
    {
        options.Endpoint = "http://localhost:4318/v1/logs";
        options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.HttpProtobuf;
        options.ResourceAttributes = new Dictionary<string, object>
        {
            ["service.name"] = "Restaurant.WebApi",
            ["service.version"] = "1.0.0"
        };
    })
    .CreateLogger();

try
{
    Log.Information("Starting Restaurant API application");

    var builder = WebApplication.CreateBuilder(args);

    // Use Serilog for logging
    builder.Host.UseSerilog();

    // Add services to the container
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() {
            Title = "Restaurant API",
            Version = "v1",
            Description = "API per la gestione di tavoli e prenotazioni del ristorante con Serilog e OpenTelemetry"
        });
    });

    // Database Configuration
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Host=localhost;Port=5432;Database=ristorante_db;Username=postgres;Password=postgres";

    builder.Services.AddDbContext<RestaurantDbContext>(options =>
        options.UseNpgsql(connectionString)
            .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
            .EnableDetailedErrors(builder.Environment.IsDevelopment()));

    // Dependency Injection - Hexagonal Architecture
    builder.Services.AddScoped<ITableRepository, TableRepository>();
    builder.Services.AddScoped<ITableService, TableService>();
    builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
    builder.Services.AddScoped<IReservationService, ReservationService>();

    // OpenTelemetry Configuration
    var serviceName = "Restaurant.WebApi";
    var serviceVersion = "1.0.0";

    builder.Services.AddOpenTelemetry()
        .ConfigureResource(resource => resource
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
            .AddAttributes(new Dictionary<string, object>
            {
                ["deployment.environment"] = builder.Environment.EnvironmentName,
                ["host.name"] = Environment.MachineName
            }))
        .WithTracing(tracing => tracing
            .AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
                options.Filter = (httpContext) =>
                {
                    return !httpContext.Request.Path.StartsWithSegments("/health");
                };
                options.EnrichWithHttpRequest = (activity, httpRequest) =>
                {
                    activity.SetTag("http.client_ip", httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString());
                    activity.SetTag("http.user_agent", httpRequest.Headers.UserAgent.ToString());
                };
                options.EnrichWithHttpResponse = (activity, httpResponse) =>
                {
                    activity.SetTag("http.response_content_length", httpResponse.ContentLength);
                };
            })
            .AddHttpClientInstrumentation(options =>
            {
                options.RecordException = true;
            })
            .AddEntityFrameworkCoreInstrumentation(options =>
            {
                options.SetDbStatementForText = true;
                options.EnrichWithIDbCommand = (activity, command) =>
                {
                    activity.SetTag("db.operation", command.CommandText);
                };
            })
            .AddSource(serviceName)
            .AddConsoleExporter()
            //.AddOtlpExporter(options =>
            //{
            //    options.Endpoint = new Uri("http://localhost:4317");
            //})
        )
        .WithMetrics(metrics => metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddMeter(serviceName)
            .AddConsoleExporter()
            //.AddOtlpExporter(options =>
            //{
            //    options.Endpoint = new Uri("http://localhost:4317");
            //})
        );

    // Health Checks
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<RestaurantDbContext>();

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    // Initialize database
    await DbInitializer.InitializeAsync(app.Services);

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API v1");
            c.RoutePrefix = string.Empty; // Swagger UI at root
        });
    }

    // Add Serilog request logging
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("RemoteIp", httpContext.Connection.RemoteIpAddress);
        };
    });

    app.UseHttpsRedirection();

    app.UseCors();

    app.UseAuthorization();

    app.MapControllers();

    app.MapHealthChecks("/health");

    // Log startup information
    Log.Information("Restaurant API started successfully");
    Log.Information("Environment: {Environment}", app.Environment.EnvironmentName);
    Log.Information("OpenTelemetry configured for service: {ServiceName} v{ServiceVersion}",
        serviceName, serviceVersion);
    Log.Information("Serilog configured with Console and File sinks");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
