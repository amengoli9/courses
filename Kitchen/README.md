# Restaurant Menu API

REST API per la gestione del menu di un ristorante, costruita con .NET 8, PostgreSQL, Entity Framework Core e OpenTelemetry, seguendo l'architettura esagonale (Ports & Adapters).

## 🏗️ Architettura Esagonale

Il progetto segue il pattern dell'architettura esagonale per separare la logica di business dalle dipendenze esterne:

```
RestaurantMenu/
├── src/
│   ├── RestaurantMenu.Domain/          # Core - Entità, servizi e interfacce (Ports)
│   │   ├── Entities/
│   │   │   └── MenuItem.cs
│   │   ├── Enums/
│   │   │   └── MenuCategory.cs
│   │   ├── Repositories/
│   │   │   └── IMenuItemRepository.cs
│   │   └── Services/
│   │       ├── IMenuItemService.cs
│   │       └── MenuItemService.cs
│   │
│   ├── RestaurantMenu.Infrastructure/  # Adapters - Implementazioni concrete
│   │   ├── Data/
│   │   │   ├── RestaurantDbContext.cs
│   │   │   └── DbInitializer.cs
│   │   ├── Configurations/
│   │   │   └── MenuItemConfiguration.cs
│   │   └── Repositories/
│   │       └── MenuItemRepository.cs
│   │
│   └── RestaurantMenu.WebApi/          # Entry Point - Controllers e DTOs
│       ├── Controllers/
│       │   └── MenuController.cs
│       ├── DTOs/
│       │   ├── MenuItemDto.cs
│       │   ├── CreateMenuItemRequest.cs
│       │   └── UpdateMenuItemRequest.cs
│       ├── Validators/
│       │   ├── CreateMenuItemRequestValidator.cs
│       │   └── UpdateMenuItemRequestValidator.cs
│       ├── Program.cs
│       └── appsettings.json
│
├── docker-compose.yml
└── RestaurantMenu.sln
```

### Layers Spiegati

- **Domain Layer**: Contiene le entità di business, i servizi di dominio e le interfacce (ports). Non ha dipendenze esterne.
- **Infrastructure Layer**: Implementa i ports definiti nel Domain (adapters). Contiene EF Core, repository, database.
- **WebApi Layer**: Entry point dell'applicazione. Contiene controller, DTOs, validators, configurazione e OpenTelemetry.

## ✨ Caratteristiche

- ✅ **Architettura Esagonale** - Separazione chiara tra business logic e infrastruttura
- ✅ **Domain-Driven Design** - Servizi e logica di business nel Domain layer
- ✅ **REST API** - Endpoint completi per CRUD operations
- ✅ **PostgreSQL** - Database relazionale con Entity Framework Core
- ✅ **OpenTelemetry** - Strumentazione completa per traces, metrics e logging
- ✅ **FluentValidation** - Validazione delle richieste nel layer WebApi
- ✅ **Swagger/OpenAPI** - Documentazione API interattiva
- ✅ **Docker Compose** - Configurazione per PostgreSQL, Jaeger e pgAdmin
- ✅ **Health Checks** - Monitoraggio dello stato dell'applicazione
- ✅ **Seed Data** - Dati di esempio pre-caricati

## 📋 Requisiti

- .NET 8.0 SDK
- Docker e Docker Compose (per database e observability stack)

## 🚀 Quick Start

### 1. Avvia l'infrastruttura con Docker

```bash
cd RestaurantMenu
docker-compose up -d
```

Questo avvierà:
- **PostgreSQL** su `localhost:5432`
- **Jaeger UI** su `http://localhost:16686` (per traces)
- **pgAdmin** su `http://localhost:5050` (per gestione database)

### 2. Ripristina le dipendenze

```bash
dotnet restore
```

### 3. Esegui l'applicazione

```bash
cd src/RestaurantMenu.WebApi
dotnet run
```

L'API sarà disponibile su:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001
- **Swagger UI**: http://localhost:5000 (root)

### 4. Esplora l'API

Apri il browser su http://localhost:5000 per accedere a Swagger UI e testare gli endpoint.

## 🔌 API Endpoints

### Menu Items

| Metodo | Endpoint | Descrizione |
|--------|----------|-------------|
| GET | `/api/menu` | Ottiene tutti gli item del menu |
| GET | `/api/menu/{id}` | Ottiene un item specifico per ID |
| GET | `/api/menu/available` | Ottiene solo gli item disponibili |
| GET | `/api/menu/category/{category}` | Ottiene item per categoria |
| POST | `/api/menu` | Crea un nuovo item |
| PUT | `/api/menu/{id}` | Aggiorna un item esistente |
| DELETE | `/api/menu/{id}` | Elimina un item |

### Health Check

| Metodo | Endpoint | Descrizione |
|--------|----------|-------------|
| GET | `/health` | Stato dell'applicazione e database |

## 📊 Categorie Menu

```csharp
public enum MenuCategory
{
    Antipasti = 1,
    PrimiPiatti = 2,
    SecondiPiatti = 3,
    Contorni = 4,
    Dolci = 5,
    Bevande = 6,
    Vini = 7
}
```

## 📝 Esempi di Richieste

### Creare un nuovo item del menu

```bash
curl -X POST http://localhost:5000/api/menu \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Pizza Margherita",
    "description": "Pizza classica con pomodoro, mozzarella e basilico",
    "price": 12.50,
    "category": 2,
    "isAvailable": true,
    "allergens": ["Glutine", "Latticini"],
    "preparationTimeMinutes": 15
  }'
```

### Ottenere tutti gli item

```bash
curl http://localhost:5000/api/menu
```

### Ottenere item per categoria (Dolci)

```bash
curl http://localhost:5000/api/menu/category/5
```

### Aggiornare un item

```bash
curl -X PUT http://localhost:5000/api/menu/{id} \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Pizza Margherita DOP",
    "description": "Pizza con ingredienti DOP certificati",
    "price": 15.00,
    "category": 2,
    "isAvailable": true,
    "allergens": ["Glutine", "Latticini"],
    "preparationTimeMinutes": 15
  }'
```

### Eliminare un item

```bash
curl -X DELETE http://localhost:5000/api/menu/{id}
```

## 📊 OpenTelemetry - Observability

### Traces

L'applicazione è completamente instrumentata con OpenTelemetry per tracciare:

- **HTTP Requests**: Tutte le richieste API con metadati (IP, User-Agent, status code, etc.)
- **Database Operations**: Query EF Core con statement SQL
- **Custom Activities**: Operazioni specifiche del business logic
- **Exceptions**: Tracciamento automatico delle eccezioni

#### Configurazione

```csharp
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .AddSource("RestaurantMenu.WebApi")
        .AddConsoleExporter()
    )
```

#### Visualizzare Traces con Jaeger

1. Apri http://localhost:16686
2. Seleziona "RestaurantMenu.WebApi" dal dropdown Service
3. Clicca "Find Traces"

### Metrics

Metriche automatiche raccolte:

- **ASP.NET Core**: Request duration, request count, etc.
- **HTTP Client**: Outbound HTTP request metrics
- **Runtime**: GC, thread pool, exception metrics

### Custom Spans

Il controller crea custom spans per ogni operazione:

```csharp
using var activity = ActivitySource.StartActivity("GetMenuItemById");
activity?.SetTag("menu_item.id", id.ToString());
activity?.SetTag("menu_item.name", item.Name);
```

## 🗄️ Database

### Connection String

La connection string di default è configurata in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=restaurant_db;Username=postgres;Password=postgres"
  }
}
```

### Migrazioni

Le migrazioni vengono applicate automaticamente all'avvio dell'applicazione grazie a `DbInitializer`.

Per creare nuove migrazioni manualmente:

```bash
cd src/RestaurantMenu.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../RestaurantMenu.WebApi
```

### Seed Data

Il database viene inizializzato con 4 item di esempio:
- Bruschetta al Pomodoro (Antipasti)
- Spaghetti alla Carbonara (Primi Piatti)
- Tagliata di Manzo (Secondi Piatti)
- Tiramisù (Dolci)

### pgAdmin

Per gestire il database visivamente:

1. Apri http://localhost:5050
2. Login: `admin@restaurant.com` / `admin`
3. Aggiungi server:
   - Host: `postgres` (nome del container)
   - Port: `5432`
   - Username: `postgres`
   - Password: `postgres`

## 🧪 Testing con Swagger

1. Apri http://localhost:5000
2. Espandi un endpoint (es. `GET /api/menu`)
3. Clicca "Try it out"
4. Clicca "Execute"
5. Visualizza la risposta

## 🔧 Configurazione

### Ambiente Development

In `appsettings.Development.json` puoi configurare:
- Log level più dettagliati
- SQL query logging
- Sensitive data logging

### Ambiente Production

Per produzione, modifica `appsettings.json`:
- Disabilita sensitive data logging
- Configura OTLP exporter per Jaeger/Grafana
- Usa connection string sicura da variabili d'ambiente

### Abilitare OTLP Exporter

Nel file `Program.cs`, decommenta:

```csharp
.AddOtlpExporter(options =>
{
    options.Endpoint = new Uri("http://localhost:4317");
})
```

## 📦 Pacchetti Utilizzati

### OpenTelemetry (v1.13.0)

- `OpenTelemetry` (1.13.0)
- `OpenTelemetry.Exporter.Console` (1.13.0)
- `OpenTelemetry.Exporter.OpenTelemetryProtocol` (1.13.0)
- `OpenTelemetry.Extensions.Hosting` (1.13.0)
- `OpenTelemetry.Instrumentation.AspNetCore` (1.13.0)
- `OpenTelemetry.Instrumentation.Http` (1.13.0)
- `OpenTelemetry.Instrumentation.Runtime` (1.13.0)
- `OpenTelemetry.Instrumentation.EntityFrameworkCore` (1.0.0-beta.12)

### Altri Pacchetti

- `Entity Framework Core` (8.0.0)
- `Npgsql.EntityFrameworkCore.PostgreSQL` (8.0.0)
- `FluentValidation` (11.9.0)
- `Swashbuckle.AspNetCore` (6.5.0)

## 🏛️ Principi dell'Architettura Esagonale

### Ports (Interfacce)

Definiti nel **Domain Layer**:
```csharp
public interface IMenuItemRepository
{
    Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    // ... altri metodi
}

public interface IMenuItemService
{
    Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    // ... altri metodi
}
```

### Adapters (Implementazioni)

**Repository** implementato nell'**Infrastructure Layer**:
```csharp
public class MenuItemRepository : IMenuItemRepository
{
    private readonly RestaurantDbContext _context;
    // ... implementazione con EF Core
}
```

**Service** implementato nel **Domain Layer**:
```csharp
public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _repository;
    // ... logica di business
}
```

### Dependency Injection

Nel **WebApi Layer** (`Program.cs`):
```csharp
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
```

### DTOs e Mapping

I **DTOs** sono nel **WebApi Layer** e vengono mappati alle entità nel controller:
```csharp
private static MenuItemDto MapToDto(MenuItem entity)
{
    return new MenuItemDto(
        entity.Id,
        entity.Name,
        // ... altri campi
    );
}
```

### Vantaggi

- ✅ **Testabilità**: Il core business è testabile senza dipendenze esterne
- ✅ **Manutenibilità**: Ogni layer ha responsabilità chiare
- ✅ **Sostituibilità**: Facile sostituire implementazioni (es. cambiare DB da PostgreSQL a MongoDB)
- ✅ **Indipendenza**: Il dominio non dipende da framework o tecnologie specifiche
- ✅ **Semplicità**: Meno layer = meno complessità, logica di business centralizzata nel Domain

## 🛠️ Comandi Utili

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run --project src/RestaurantMenu.WebApi
```

### Watch (auto-reload)

```bash
dotnet watch --project src/RestaurantMenu.WebApi
```

### Clean

```bash
dotnet clean
```

### Restore

```bash
dotnet restore
```

## 🐳 Docker Commands

### Start services

```bash
docker-compose up -d
```

### Stop services

```bash
docker-compose down
```

### View logs

```bash
docker-compose logs -f
```

### Restart PostgreSQL

```bash
docker-compose restart postgres
```

## 📚 Prossimi Passi

- [ ] Aggiungere autenticazione e autorizzazione (JWT)
- [ ] Implementare paginazione per GET /api/menu
- [ ] Aggiungere filtri avanzati (prezzo, allergeni, tempo preparazione)
- [ ] Implementare caching con Redis
- [ ] Aggiungere unit tests e integration tests
- [ ] Implementare CQRS pattern
- [ ] Aggiungere rate limiting
- [ ] Implementare soft delete
- [ ] Aggiungere audit trail
- [ ] Configurare CI/CD pipeline

## 📖 Riferimenti

- [Hexagonal Architecture](https://alistair.cockburn.us/hexagonal-architecture/)
- [OpenTelemetry .NET](https://opentelemetry.io/docs/instrumentation/net/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
- [FluentValidation](https://docs.fluentvalidation.net/)
- [Npgsql PostgreSQL](https://www.npgsql.org/efcore/)
- [Jaeger Tracing](https://www.jaegertracing.io/)

## 📄 Licenza

Questo progetto è un esempio educativo per dimostrare architettura esagonale e OpenTelemetry.

## 👨‍💻 Autore

Creato come esempio di implementazione di architettura esagonale con .NET 8, PostgreSQL e OpenTelemetry.
