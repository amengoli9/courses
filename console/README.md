# Console Application with OpenTelemetry

Una console application .NET 8 con strumentazione OpenTelemetry completa per traces, metrics e logging.

## Caratteristiche

- ✅ **Traces**: Distributed tracing con ActivitySource personalizzato
- ✅ **Metrics**: Metriche custom (Counter, Histogram) per monitorare performance
- ✅ **Runtime Instrumentation**: Monitoraggio automatico delle metriche runtime .NET
- ✅ **HTTP Instrumentation**: Tracciamento automatico delle chiamate HTTP
- ✅ **Console Exporter**: Output di traces e metrics direttamente sulla console
- ✅ **OTLP Exporter**: Pronto per l'integrazione con Jaeger, Zipkin, Grafana, etc.

## Requisiti

- .NET 8.0 SDK o superiore

## OpenTelemetry Packages (v1.13.0)

Il progetto include i seguenti pacchetti OpenTelemetry:

- `OpenTelemetry` (1.13.0) - Core library
- `OpenTelemetry.Api` (1.13.0) - API per creare spans e metrics custom
- `OpenTelemetry.Exporter.Console` (1.13.0) - Esportatore per console
- `OpenTelemetry.Exporter.OpenTelemetryProtocol` (1.13.0) - Esportatore OTLP
- `OpenTelemetry.Extensions.Hosting` (1.13.0) - Integrazione con hosting
- `OpenTelemetry.Instrumentation.Runtime` (1.13.0) - Metriche runtime .NET
- `OpenTelemetry.Instrumentation.Http` (1.13.0) - Tracciamento HTTP automatico

## Installazione e Esecuzione

```bash
# Ripristina le dipendenze
dotnet restore

# Compila il progetto
dotnet build

# Esegui l'applicazione
dotnet run
```

## Struttura del Codice

### ActivitySource e Meter

```csharp
// ActivitySource per custom tracing
private static readonly ActivitySource ActivitySource = new("ConsoleApp", "1.0.0");

// Meter per custom metrics
private static readonly Meter Meter = new("ConsoleApp", "1.0.0");
```

### Metriche Custom

- **app.requests**: Counter che traccia il numero totale di richieste processate
- **app.request.duration**: Histogram che misura la durata delle operazioni in millisecondi

### Spans/Activities Custom

L'applicazione crea diversi spans per tracciare il workflow:

1. `ApplicationWorkflow` - Span root principale
2. `ProcessUserRequest` - Elaborazione richiesta utente
3. `PerformCalculation` - Operazioni di calcolo
4. `SimulateHttpCall` - Chiamate HTTP simulate

## Configurazione OpenTelemetry

### Traces Configuration

```csharp
var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService(serviceName: "ConsoleApp", serviceVersion: "1.0.0"))
    .AddSource(ActivitySource.Name)
    .AddHttpClientInstrumentation()
    .AddConsoleExporter()
    .Build();
```

### Metrics Configuration

```csharp
var meterProvider = Sdk.CreateMeterProviderBuilder()
    .SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService(serviceName: "ConsoleApp", serviceVersion: "1.0.0"))
    .AddMeter(Meter.Name)
    .AddRuntimeInstrumentation()
    .AddConsoleExporter()
    .Build();
```

## Integrazione con Backend di Observability

Per inviare i dati a un backend come Jaeger, Zipkin, o un OTLP collector, sostituisci o aggiungi l'esportatore OTLP:

```csharp
.AddOtlpExporter(options =>
{
    options.Endpoint = new Uri("http://localhost:4317"); // gRPC endpoint
    // oppure
    // options.Endpoint = new Uri("http://localhost:4318"); // HTTP endpoint
})
```

### Esempi di Backend

#### Jaeger (con Docker)

```bash
docker run -d --name jaeger \
  -p 16686:16686 \
  -p 4317:4317 \
  -p 4318:4318 \
  jaegertracing/all-in-one:latest
```

Poi apri http://localhost:16686 per vedere le traces.

#### Zipkin (con Docker)

```bash
docker run -d --name zipkin \
  -p 9411:9411 \
  openzipkin/zipkin:latest
```

Poi apri http://localhost:9411 per vedere le traces.

## Output di Esempio

Quando esegui l'applicazione, vedrai output simile a:

```
=== Console Application with OpenTelemetry ===

OpenTelemetry instrumentation configured successfully!

Starting application workflow...

Step 1: Processing user request...
Step 2: Performing calculation...
Step 3: Simulating HTTP call...

Application completed. Check the traces and metrics above!

[Traces export]
Activity.TraceId:            ...
Activity.SpanId:             ...
Activity.TraceFlags:         Recorded
Activity.ParentSpanId:       ...
Activity.ActivitySourceName: ConsoleApp
Activity.DisplayName:        ProcessUserRequest
Activity.Kind:               Internal
Activity.StartTime:          ...
Activity.Duration:           ...

[Metrics export]
Meter: ConsoleApp/1.0.0
  Instrument: app.requests
    Counter: 3
  Instrument: app.request.duration
    Histogram: ...
```

## Best Practices Implementate

1. **Resource Attribution**: Ogni span e metrica è associata al servizio "ConsoleApp"
2. **Structured Tags**: Utilizzo di tag semantici (user.id, http.method, etc.)
3. **Activity Events**: Registrazione di eventi significativi durante l'esecuzione
4. **Exception Recording**: Tracciamento automatico delle eccezioni
5. **Status Codes**: Utilizzo di ActivityStatusCode per indicare successo/errore
6. **Proper Disposal**: Uso di `using` statements per cleanup automatico

## Prossimi Passi

- Aggiungi logging strutturato con OpenTelemetry.Logs
- Integra con database (usa OpenTelemetry.Instrumentation.SqlClient)
- Aggiungi instrumentazione per gRPC o altri protocolli
- Configura sampling per produzione
- Implementa baggage propagation per context sharing

## Riferimenti

- [OpenTelemetry .NET Documentation](https://opentelemetry.io/docs/instrumentation/net/)
- [OpenTelemetry .NET GitHub](https://github.com/open-telemetry/opentelemetry-dotnet)
- [OpenTelemetry Specification](https://opentelemetry.io/docs/specs/otel/)
