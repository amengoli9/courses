# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Overview

This is an educational microservices project demonstrating .NET observability patterns with OpenTelemetry and Serilog. The repository contains two main microservices (Restaurant and Kitchen) that communicate via HTTP, plus supporting libraries and example applications.

## Build and Run Commands

### Build entire solution
```bash
cd Observability
dotnet build Observability.sln
```

### Build individual services
```bash
# Kitchen service
cd Observability/Kitchen
dotnet build Kitchen.sln

# Restaurant service
cd Observability/Restaurant
dotnet build Restaurant.sln

# Common logging library
cd Observability/Common.Logging
dotnet build Common.Logging.sln
```

### Run services

**Important**: Services must be started in the correct order:

1. Start infrastructure (Docker):
```bash
cd Observability/Restaurant
docker-compose up -d postgres jaeger seq
```

2. Start Kitchen service (port 5000):
```bash
cd Observability/Kitchen/src/Kitchen.WebApi
dotnet run
```

3. Start Restaurant service (port 5001):
```bash
cd Observability/Restaurant/src/Restaurant.WebApi
dotnet run
```

### Test distributed tracing
```bash
# Call Restaurant, which proxies to Kitchen
curl http://localhost:5001/api/menu

# View traces in Jaeger UI
open http://localhost:16686

# View structured logs in Seq UI
open http://localhost:5341
```

### Run example applications
```bash
# Console app with custom OpenTelemetry instrumentation
cd Observability/console
dotnet run
```

## Architecture

### Microservices Communication Pattern

The architecture demonstrates distributed tracing across service boundaries:

```
Client → Restaurant (port 5001) → Kitchen (port 5000) → PostgreSQL
         ├─ Serilog (4 sinks)    └─ OpenTelemetry
         └─ OpenTelemetry
```

**Key flow**: When a client calls `GET /api/menu` on Restaurant, the Restaurant service makes an HTTP call to Kitchen's `GET /api/menu` endpoint. OpenTelemetry automatically propagates the trace context via HTTP headers, creating a single distributed trace across both services.

### Hexagonal Architecture (3 Layers)

Both microservices follow hexagonal architecture with clear separation:

- **WebApi Layer**: Controllers, DTOs, validation, HTTP concerns
- **Domain Layer**: Business logic, domain services (e.g., `IMenuItemService`), domain entities, repository interfaces
- **Infrastructure Layer**: Repository implementations, DbContext, external service clients (e.g., `KitchenApiService`)

**Dependency flow**: WebApi → Domain ← Infrastructure (Domain defines interfaces, Infrastructure implements them)

### OpenTelemetry Instrumentation

Both services are instrumented with:
- **ASP.NET Core Instrumentation**: Automatically traces HTTP requests
- **HttpClient Instrumentation**: Traces outbound HTTP calls with automatic trace propagation
- **Entity Framework Core Instrumentation**: Traces database queries with SQL statements
- **Custom spans**: Can add custom `ActivitySource` spans for business logic

Traces export to console (development) and can be configured to export to OTLP collector (Jaeger) by uncommenting the `.AddOtlpExporter()` configuration in Program.cs.

### Serilog Configuration (Restaurant Service Only)

Restaurant service uses Serilog with 4 sinks:
1. **Console**: Colored output for development
2. **File**: Daily rolling logs in `logs/restaurant-{Date}.log`
3. **Seq**: Structured log UI at http://localhost:5341
4. **OpenTelemetry**: Correlates logs with traces via TraceId/SpanId

Kitchen service uses standard .NET logging (Console only).

### Common.Logging Library

Shared library using source-generated logging (`LoggerMessage` attribute) for zero-allocation structured logging. Provides standardized log messages with EventIds for:
- HTTP operations (1000-1099)
- Database operations (2000-2099)
- Business operations (3000-3099)

Use these extension methods instead of string interpolation:
```csharp
logger.CallingMicroservice("Kitchen", "GetMenu", traceId);
logger.EntityCreated("MenuItem", menuItemId);
```

## Database Management

### Connection strings
- Kitchen: `Host=localhost;Port=5432;Database=restaurant_db;Username=postgres;Password=postgres`
- Restaurant: `Host=localhost;Port=5432;Database=ristorante_db;Username=postgres;Password=postgres`

### Database initialization
Both services auto-initialize databases on startup via `DbInitializer.InitializeAsync()`. The initializer:
- Creates database if it doesn't exist
- Applies migrations
- Seeds initial data (menu items, tables)

### Docker infrastructure
```bash
# Start all infrastructure
cd Observability/Restaurant
docker-compose up -d

# Check service status
docker-compose ps

# View logs
docker-compose logs -f postgres
docker-compose logs -f jaeger
docker-compose logs -f seq

# Stop infrastructure
docker-compose down
```

## Service Endpoints

### Kitchen Service (port 5000)
- `GET /api/menu` - Get all menu items
- `GET /api/menu/{id}` - Get menu item by ID
- `POST /api/menu` - Create menu item
- `PUT /api/menu/{id}` - Update menu item
- `DELETE /api/menu/{id}` - Delete menu item
- Swagger UI: http://localhost:5000

### Restaurant Service (port 5001)
- `GET /api/menu` - Proxies to Kitchen service
- `GET /api/tables` - Get all tables
- `GET /api/tables/{id}` - Get table by ID
- `POST /api/tables` - Create table
- `PUT /api/tables/{id}` - Update table
- `DELETE /api/tables/{id}` - Delete table
- `GET /health` - Health check
- Swagger UI: http://localhost:5001

## Observability Stack

### Jaeger (Distributed Tracing)
- URL: http://localhost:16686
- OTLP gRPC: port 4317
- OTLP HTTP: port 4318
- Search for service "Restaurant.WebApi" or "Kitchen.WebApi" to see traces

### Seq (Structured Logs)
- URL: http://localhost:5341
- Query logs using SQL-like syntax
- Filter by TraceId to correlate logs with traces
- Restaurant service only (Kitchen doesn't send logs to Seq)

### pgAdmin (Database Admin)
- URL: http://localhost:5050
- Login: admin@restaurant.com / admin
- Available when using Restaurant's docker-compose

## Common Development Patterns

### Adding a new entity with distributed tracing

When creating a new entity/feature across services:

1. **Domain layer**: Define entity and service interface
2. **Infrastructure layer**: Implement repository with EF Core (automatic query tracing)
3. **WebApi layer**: Add controller (automatic HTTP tracing)
4. **Logging**: Use Common.Logging extension methods for structured logs
5. **Custom spans**: Add `ActivitySource` spans for complex business logic:
   ```csharp
   using var activity = ActivitySource.StartActivity("BusinessOperation");
   activity?.SetTag("entity.id", entityId);
   ```

### Inter-service communication

When calling between services (like Restaurant → Kitchen):

1. Register `HttpClient` with DI:
   ```csharp
   builder.Services.AddHttpClient<IServiceInterface, ServiceImpl>(client => {
       client.BaseAddress = new Uri("http://localhost:5000");
   });
   ```
2. OpenTelemetry automatically propagates `traceparent` header
3. Use Common.Logging methods for logging calls:
   ```csharp
   logger.CallingMicroservice(targetService, operation, traceId);
   ```

### Handling service failures

Restaurant service demonstrates resilience when Kitchen is unavailable:
- Returns appropriate error responses
- Logs errors with structured data
- Traces show failed HTTP calls with error details
- Consider adding retry policies or circuit breakers for production

## Project Structure Note

The git status shows deleted files from root-level Kitchen/, Restaurant/, console/, and Common.Logging/ directories. The active code is in the Observability/ directory, which contains the current solution structure.
