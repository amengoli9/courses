# Restaurant API

REST API per la gestione di tavoli e prenotazioni di un ristorante, costruita con .NET 8, **Serilog**, PostgreSQL, Entity Framework Core e OpenTelemetry.

## 🎯 Focus: Logging Strutturato con Serilog + OpenTelemetry

Questo progetto dimostra l'uso di **Serilog** per logging strutturato avanzato con integrazione **OpenTelemetry Sink** in un'applicazione .NET 8.

### Configurazione Serilog con 4 Sinks

Serilog è configurato in `Program.cs` con **4 sinks simultanei**:

1. **Console Sink**: Output formattato sulla console per sviluppo
2. **File Sink**: Log persistenti con rolling giornaliero (`logs/restaurant-YYYY-MM-DD.log`)
3. **Seq Sink**: Piattaforma di ricerca e analisi log (http://localhost:5341)
4. **OpenTelemetry Sink**: Invio log al collector OTLP (http://localhost:4318)

```csharp
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
        retainedFileCountLimit: 30)
    .WriteTo.Seq("http://localhost:5341")
    .WriteTo.OpenTelemetry(options =>
    {
        options.Endpoint = "http://localhost:4318/v1/logs";
        options.Protocol = OtlpProtocol.HttpProtobuf;
        options.ResourceAttributes = new Dictionary<string, object>
        {
            ["service.name"] = "Restaurant.WebApi",
            ["service.version"] = "1.0.0"
        };
    })
    .CreateLogger();
```

### OpenTelemetry Sink - Unified Observability

Il **Serilog.Sinks.OpenTelemetry** permette di inviare i log strutturati direttamente al collector OpenTelemetry, integrandoli con traces e metrics:

- **Endpoint**: `http://localhost:4318/v1/logs` (OTLP HTTP)
- **Protocol**: HttpProtobuf
- **Resource Attributes**: service.name, service.version
- **Integration**: I log appaiono in Jaeger insieme alle traces!

### Request Logging

Serilog traccia automaticamente tutte le richieste HTTP con enrichment:

```csharp
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
```

### Logging nei Controller

```csharp
_logger.LogInformation("Fetching table {TableId}", id);
_logger.LogWarning("Table {TableId} not found", id);
_logger.LogError(ex, "Error creating table");
```

## 🏗️ Architettura (Hexagonal - 3 Layers)

- **Domain**: Entità (Table, Reservation), Servizi, Repository interfaces
- **Infrastructure**: EF Core, PostgreSQL, Repository implementations
- **WebApi**: Controllers, DTOs, Serilog, OpenTelemetry

## 🚀 Quick Start

### 1. Avvia infrastruttura

```bash
cd Restaurant
docker-compose up -d
```

Questo avvia:
- **PostgreSQL** (porta 5432)
- **Jaeger** (porta 16686) - Traces & Logs visualization
- **Seq** (porta 5341) - Log search & analytics

### 2. Esegui l'applicazione

```bash
cd src/Restaurant.WebApi
dotnet run
```

L'API sarà disponibile su:
- **HTTP**: http://localhost:5100
- **HTTPS**: https://localhost:5101
- **Swagger**: http://localhost:5100

## 📊 Visualizzare i Log

### Console

Log appaiono direttamente nel terminale:
```
[14:32:15 INF] Fetching all tables
[14:32:15 INF] Retrieved 3 tables
```

### File

Log salvati in `logs/restaurant-2025-11-02.log`:
```
2025-11-02 14:32:15.123 +00:00 [INF] HTTP GET /api/tables responded 200 in 45.2341 ms
```

### Seq (Structured Logs UI)

1. Apri http://localhost:5341
2. Query esempi:
   - `StatusCode >= 400` - Errori
   - `TableId = '...'` - Filtra per tavolo
   - `@Level = 'Error'` - Solo errori

### Jaeger (Traces + Logs)

1. Apri http://localhost:16686
2. Seleziona "Restaurant.WebApi"
3. Vedi traces con **logs correlati** grazie al sink OpenTelemetry!

## 🔌 API Endpoints

| Endpoint | Metodo | Descrizione |
|----------|--------|-------------|
| `/api/tables` | GET | Lista tutti i tavoli |
| `/api/tables/{id}` | GET | Dettaglio tavolo |
| `/api/tables` | POST | Crea nuovo tavolo |
| `/health` | GET | Health check |

## 📦 Pacchetti Serilog

```xml
<PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
<PackageReference Include="Serilog.Sinks.Console" Version="5.0.1" />
<PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
<PackageReference Include="Serilog.Sinks.Seq" Version="7.0.1" />
<PackageReference Include="Serilog.Sinks.OpenTelemetry" Version="4.1.0" /> ⭐
<PackageReference Include="Serilog.Enrichers.Environment" Version="3.0.1" />
<PackageReference Include="Serilog.Enrichers.Thread" Version="4.0.0" />
<PackageReference Include="Serilog.Settings.Configuration" Version="8.0.0" />
```

## 🎨 Output Templates

### Console (Development)
```
[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}
```

### File (Production)
```
{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}
```

## 🔧 Configurazione OpenTelemetry Sink

### Endpoint
- **OTLP HTTP**: `http://localhost:4318/v1/logs`
- **Protocol**: HttpProtobuf
- **Resource Attributes**: Automaticamente aggiunti service.name e service.version

### Vantaggi

1. **Unified Observability**: Logs, traces e metrics nello stesso collector
2. **Correlation**: Log correlati automaticamente alle traces
3. **Standard**: Formato OTLP universale
4. **Performance**: Invio batch asincrono
5. **Scalability**: Pronto per produzione con collector OTLP

## 🐳 Docker Services

```yaml
services:
  postgres:      # PostgreSQL database
  jaeger:        # Traces + Logs visualization
  seq:           # Structured log search
```

## ✨ Caratteristiche Serilog

1. **4 Sinks Simultanei**: Console + File + Seq + OpenTelemetry
2. **Logging Strutturato**: Proprietà tipizzate interrogabili
3. **Enrichment**: MachineName, ThreadId, Environment
4. **Rolling Files**: Retention 30 giorni
5. **Request Logging**: HTTP middleware automatico
6. **Level Filtering**: Per namespace
7. **OpenTelemetry Integration**: ⭐ Logs nel collector OTLP

## 📚 Esempi di Log Strutturato

```csharp
// ❌ String interpolation
_logger.LogInformation($"User {userId} created order {orderId}");

// ✅ Structured logging
_logger.LogInformation("User {UserId} created order {OrderId}", userId, orderId);
```

Con il sink OpenTelemetry, questi log appaiono in Jaeger correlati alle traces!

## 🛠️ Comandi Utili

```bash
# Build
dotnet build

# Run
dotnet run --project src/Restaurant.WebApi

# Watch
dotnet watch --project src/Restaurant.WebApi

# Docker
docker-compose up -d
docker-compose logs -f
```

## 📖 Riferimenti

- [Serilog](https://serilog.net/)
- [Serilog.Sinks.OpenTelemetry](https://github.com/serilog/serilog-sinks-opentelemetry)
- [Seq](https://datalust.co/seq)
- [OpenTelemetry](https://opentelemetry.io/)
- [Jaeger](https://www.jaegertracing.io/)
