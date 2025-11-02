# Ristorante API

REST API per la gestione di tavoli e prenotazioni di un ristorante, costruita con .NET 8, **Serilog**, PostgreSQL, Entity Framework Core e OpenTelemetry.

## 🎯 Focus: Logging Strutturato con Serilog

Questo progetto dimostra l'uso di **Serilog** per logging strutturato e avanzato in un'applicazione .NET 8.

### Configurazione Serilog

Serilog è configurato in `Program.cs` con:

- **Console Sink**: Output formattato sulla console per sviluppo
- **File Sink**: Log su file con rolling giornaliero (`logs/ristorante-YYYY-MM-DD.log`)
- **Seq Sink** (opzionale): Piattaforma di ricerca e analisi log
- **Enrichers**: Arricchimento automatico con MachineName, ThreadId, Environment

```csharp
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/ristorante-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)
    .WriteTo.Seq("http://localhost:5341")  // Opzionale
    .CreateLogger();
```

### Request Logging

Serilog traccia automaticamente tutte le richieste HTTP:

```csharp
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RemoteIp", httpContext.Connection.RemoteIpAddress);
    };
});
```

### Logging nei Controller

```csharp
_logger.LogInformation("Fetching table {TableId}", id);
_logger.LogWarning("Table {TableId} not found", id);
_logger.LogError(ex, "Error creating table");
```

## 🏗️ Architettura

- **Domain**: Entità (Table, Reservation), Servizi, Repository interfaces
- **Infrastructure**: EF Core, PostgreSQL, Repository implementations
- **WebApi**: Controllers, DTOs, Serilog, OpenTelemetry

## 🚀 Quick Start

### 1. Avvia infrastruttura

```bash
docker-compose up -d
```

Questo avvia:
- **PostgreSQL** (porta 5432)
- **Jaeger** (porta 16686) - Traces
- **Seq** (porta 5341) - Logs strutturati

### 2. Esegui l'applicazione

```bash
cd src/Ristorante.WebApi
dotnet run
```

L'API sarà su:
- HTTP: http://localhost:5100
- HTTPS: https://localhost:5101
- Swagger: http://localhost:5100

## 📊 Visualizzare i Log

### Console

I log appaiono direttamente sulla console con formato:
```
[14:32:15 INF] Fetching all tables
[14:32:15 INF] Retrieved 3 tables
```

### File

Log salvati in `logs/ristorante-YYYY-MM-DD.log` con timestamp completi:
```
2025-11-02 14:32:15.123 +00:00 [INF] HTTP GET /api/tables responded 200 in 45.2341 ms
```

### Seq (UI Web)

1. Apri http://localhost:5341
2. Cerca e filtra log strutturati
3. Query avanzate: `TableId = '...'`, `StatusCode >= 400`, etc.

## 🔌 API Endpoints

| Metodo | Endpoint | Descrizione |
|--------|----------|-------------|
| GET | `/api/tables` | Lista tutti i tavoli |
| GET | `/api/tables/{id}` | Dettaglio tavolo |
| POST | `/api/tables` | Crea nuovo tavolo |
| GET | `/health` | Health check |

## 📦 Pacchetti Serilog

- `Serilog.AspNetCore` (8.0.0) - Integrazione ASP.NET Core
- `Serilog.Sinks.Console` (5.0.1) - Output console
- `Serilog.Sinks.File` (5.0.0) - Output su file
- `Serilog.Sinks.Seq` (7.0.1) - Output verso Seq
- `Serilog.Enrichers.Environment` (3.0.1) - Enricher ambiente
- `Serilog.Enrichers.Thread` (4.0.0) - Enricher thread

## 🎨 Template di Output

### Console (Development)
```
outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
```

### File (Production)
```
outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
```

## 🔧 Configurazione

### appsettings.json

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.EntityFrameworkCore": "Information"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      { "Name": "File", "Args": { "path": "logs/ristorante-.log", "rollingInterval": "Day" } }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ]
  }
}
```

## ✨ Caratteristiche Serilog

1. **Logging Strutturato**: Proprietà tipizzate invece di stringhe concatenate
2. **Sinks Multipli**: Console + File + Seq simultaneamente
3. **Enrichment Automatico**: MachineName, ThreadId, Environment
4. **Rolling Files**: Nuovi file ogni giorno, retention configurabile
5. **Performance**: Asincrono e ottimizzato
6. **Filtering Avanzato**: MinimumLevel per namespace
7. **Request Logging**: HTTP middleware automatico

## 📚 Esempi di Log Strutturato

```csharp
// ❌ String interpolation (vecchio modo)
_logger.LogInformation($"User {userId} created order {orderId}");

// ✅ Structured logging (Serilog)
_logger.LogInformation("User {UserId} created order {OrderId}", userId, orderId);
```

Con Serilog, `UserId` e `OrderId` sono proprietà interrogabili in Seq!

## 🛠️ Comandi Utili

```bash
# Build
dotnet build

# Run
dotnet run --project src/Ristorante.WebApi

# Watch
dotnet watch --project src/Ristorante.WebApi

# Docker logs
docker-compose logs -f postgres
docker-compose logs -f seq
```

## 📖 Riferimenti

- [Serilog](https://serilog.net/)
- [Seq](https://datalust.co/seq)
- [Serilog Best Practices](https://github.com/serilog/serilog/wiki/Getting-Started)
