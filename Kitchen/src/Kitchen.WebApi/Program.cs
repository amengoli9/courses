using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Kitchen.Domain.Services;
using Kitchen.Domain.Repositories;
using Kitchen.Infrastructure.Data;
using Kitchen.Infrastructure.Repositories;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() {
        Title = "Restaurant Menu API",
        Version = "v1",
        Description = "API per la gestione del menu di un ristorante con architettura esagonale e OpenTelemetry"
    });
});

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Port=5432;Database=restaurant_db;Username=postgres;Password=postgres";

builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseNpgsql(connectionString)
        .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
        .EnableDetailedErrors(builder.Environment.IsDevelopment()));

// Dependency Injection - Hexagonal Architecture
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();

// OpenTelemetry Configuration
var serviceName = "Kitchen.WebApi";
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
                // Don't trace health check endpoints
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
        .AddSource("Kitchen.WebApi")
        .AddConsoleExporter()
        // Uncomment to export to OTLP collector (e.g., Jaeger)
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
        // Uncomment to export to OTLP collector
        //.AddOtlpExporter(options =>
        //{
        //    options.Endpoint = new Uri("http://localhost:4317");
        //})
    );

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<RestaurantDbContext>();

// CORS (if needed)
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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant Menu API v1");
        c.RoutePrefix = string.Empty; // Swagger UI at root
    });
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

// Log startup information
app.Logger.LogInformation("Restaurant Menu API started successfully");
app.Logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);
app.Logger.LogInformation("OpenTelemetry configured for service: {ServiceName} v{ServiceVersion}",
    serviceName, serviceVersion);

app.Run();
