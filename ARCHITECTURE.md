# Architettura Microservizi - Restaurant & Kitchen

## Diagramma Flusso delle Chiamate

```mermaid
graph TB
    Client[Client HTTP]

    subgraph Restaurant["🍽️ Restaurant Service (Port 5001)"]
        RestAPI[WebApi Layer]
        RestDomain[Domain Layer]
        RestInfra[Infrastructure Layer]
        RestDB[(PostgreSQL<br/>restaurant_db)]

        RestAPI --> RestDomain
        RestDomain --> RestInfra
        RestInfra --> RestDB
    end

    subgraph Kitchen["👨‍🍳 Kitchen Service (Port 5000)"]
        KitchenAPI[WebApi Layer]
        KitchenDomain[Domain Layer]
        KitchenInfra[Infrastructure Layer]
        KitchenDB[(PostgreSQL<br/>kitchen_db)]

        KitchenAPI --> KitchenDomain
        KitchenDomain --> KitchenInfra
        KitchenInfra --> KitchenDB
    end

    subgraph Observability["📊 Observability Stack"]
        Jaeger[Jaeger<br/>Traces]
        Seq[Seq<br/>Logs]
        OTLP[OTLP Collector<br/>:4318]
    end

    Client -->|1. GET /api/menu| RestAPI
    Client -->|2. GET /api/tables| RestAPI

    RestInfra -->|3. HTTP GET /api/menu| KitchenAPI

    Restaurant -.->|OpenTelemetry<br/>Traces| OTLP
    Restaurant -.->|Serilog<br/>Logs| Seq
    Restaurant -.->|Serilog<br/>Logs| OTLP

    Kitchen -.->|OpenTelemetry<br/>Traces| OTLP

    OTLP -.-> Jaeger

    style Restaurant fill:#e1f5ff
    style Kitchen fill:#fff4e1
    style Observability fill:#f0f0f0
```

## Dettaglio Architettura Esagonale

```mermaid
graph LR
    subgraph "Restaurant Service"
        direction TB
        RC[Controllers]
        RD[Domain Services<br/>ITableService<br/>IReservationService<br/>IKitchenApiService]
        RI[Infrastructure<br/>Repositories<br/>KitchenApiService]

        RC --> RD
        RD --> RI
    end

    subgraph "Kitchen Service"
        direction TB
        KC[Controllers]
        KD[Domain Services<br/>IMenuItemService]
        KI[Infrastructure<br/>Repositories]

        KC --> KD
        KD --> KI
    end

    RI -->|HttpClient| KC

    style RC fill:#4CAF50
    style RD fill:#2196F3
    style RI fill:#FF9800
    style KC fill:#4CAF50
    style KD fill:#2196F3
    style KI fill:#FF9800
```

## Flusso Dettagliato: Client → Restaurant → Kitchen

```mermaid
sequenceDiagram
    participant C as Client
    participant RestCtrl as Restaurant<br/>MenuController
    participant KitchenSvc as KitchenApiService<br/>(HttpClient)
    participant KitchenCtrl as Kitchen<br/>MenuController
    participant MenuSvc as Kitchen<br/>MenuItemService
    participant Repo as Kitchen<br/>MenuItemRepository
    participant DB as PostgreSQL
    participant OTel as OpenTelemetry
    participant Serilog as Serilog

    C->>RestCtrl: GET /api/menu
    activate RestCtrl

    RestCtrl->>Serilog: Log "Fetching full menu from Kitchen API"
    RestCtrl->>OTel: Start Span "GET /api/menu"

    RestCtrl->>KitchenSvc: GetMenuAsync()
    activate KitchenSvc

    KitchenSvc->>Serilog: Log "Calling Kitchen API to get full menu"
    KitchenSvc->>OTel: Start Span "HTTP GET Kitchen"

    KitchenSvc->>KitchenCtrl: HTTP GET /api/menu
    activate KitchenCtrl

    KitchenCtrl->>MenuSvc: GetAllMenuItemsAsync()
    activate MenuSvc

    MenuSvc->>Repo: GetAllAsync()
    activate Repo

    Repo->>DB: SELECT * FROM MenuItems
    activate DB
    DB-->>Repo: ResultSet
    deactivate DB

    Repo-->>MenuSvc: List<MenuItem>
    deactivate Repo

    MenuSvc-->>KitchenCtrl: List<MenuItem>
    deactivate MenuSvc

    KitchenCtrl-->>KitchenSvc: HTTP 200 + JSON
    deactivate KitchenCtrl

    KitchenSvc->>OTel: End Span + Status
    KitchenSvc->>Serilog: Log "Retrieved N menu items"

    KitchenSvc-->>RestCtrl: List<MenuItemDto>
    deactivate KitchenSvc

    RestCtrl->>OTel: End Span + Status
    RestCtrl-->>C: HTTP 200 + JSON
    deactivate RestCtrl
```

## Stack Tecnologico

### Restaurant Service
- **Framework**: .NET 8.0 Web API
- **Database**: PostgreSQL (restaurant_db)
- **Logging**: Serilog (Console, File, Seq, OpenTelemetry)
- **Tracing**: OpenTelemetry
- **Pattern**: Hexagonal Architecture (3 layers)
- **Port**: 5001

**Entità**:
- Table (TableNumber, Capacity, Status, Location)
- Reservation (CustomerName, Email, DateTime, Status)

**Funzionalità**:
- Gestione tavoli
- Gestione prenotazioni
- Proxy per menu da Kitchen API

### Kitchen Service
- **Framework**: .NET 8.0 Web API
- **Database**: PostgreSQL (kitchen_db)
- **Logging**: Console
- **Tracing**: OpenTelemetry
- **Pattern**: Hexagonal Architecture (3 layers)
- **Port**: 5000

**Entità**:
- MenuItem (Name, Description, Price, Category, Allergens, IsAvailable, etc.)

**Funzionalità**:
- CRUD menu items
- Filtri per categoria, allergie, disponibilità

### Console App
- **Framework**: .NET 8.0 Console
- **Tracing**: OpenTelemetry (custom ActivitySource)
- **Metrics**: OpenTelemetry (custom Meter)

## Distributed Tracing

OpenTelemetry traccia automaticamente:

1. **HTTP Requests** (ASP.NET Core Instrumentation)
   - Ogni chiamata REST viene tracciata
   - Propagazione automatica del `traceparent` header

2. **HTTP Client Calls** (HttpClient Instrumentation)
   - Le chiamate da Restaurant → Kitchen vengono tracciate
   - Correlazione automatica tra i servizi

3. **Database Queries** (EF Core Instrumentation)
   - Query SQL tracciate con parametri
   - Tempo di esecuzione registrato

4. **Custom Spans** (ActivitySource)
   - Possibilità di aggiungere span personalizzati
   - Attributi custom per business logic

### Esempio Trace Distribuita

```
TraceID: 1234567890abcdef

Span 1: GET /api/menu (Restaurant) [200ms]
  │
  ├─ Span 2: KitchenApiService.GetMenuAsync [180ms]
  │   │
  │   └─ Span 3: HTTP GET http://localhost:5000/api/menu [170ms]
  │       │
  │       ├─ Span 4: MenuController.GetAllMenuItems (Kitchen) [165ms]
  │       │   │
  │       │   └─ Span 5: MenuItemService.GetAllMenuItemsAsync [160ms]
  │       │       │
  │       │       └─ Span 6: PostgreSQL SELECT MenuItems [150ms]
```

## Serilog Structured Logging

### Restaurant Service - 4 Sinks

1. **Console**: Output colorato per sviluppo
2. **File**: Rotazione giornaliera in `logs/ristorante-{Date}.log`
3. **Seq**: Dashboard web per query sui log
4. **OpenTelemetry**: Correlazione log-trace via OTLP

### Esempio Log Correlato

```json
{
  "Timestamp": "2025-11-02T10:30:45.123Z",
  "Level": "Information",
  "MessageTemplate": "Calling Kitchen API to get full menu",
  "TraceId": "1234567890abcdef",
  "SpanId": "abcdef1234567890",
  "ServiceName": "Restaurant.WebApi",
  "MachineName": "dev-machine",
  "ThreadId": 42
}
```

## Come Eseguire l'Architettura

### 1. Avvia l'infrastruttura (Docker)

```bash
docker-compose up -d postgres jaeger seq
```

### 2. Avvia Kitchen Service

```bash
cd Kitchen/src/Kitchen.WebApi
dotnet run
# Listening on http://localhost:5000
```

### 3. Avvia Restaurant Service

```bash
cd Restaurant/src/Restaurant.WebApi
dotnet run
# Listening on http://localhost:5001
```

### 4. Test chiamata distribuita

```bash
# Il Restaurant chiama automaticamente Kitchen
curl http://localhost:5001/api/menu

# Oppure chiama direttamente Kitchen
curl http://localhost:5000/api/menu
```

### 5. Visualizza telemetria

- **Jaeger UI**: http://localhost:16686 (traces distribuite)
- **Seq UI**: http://localhost:5341 (log strutturati)

## Vantaggi Architettura

✅ **Separation of Concerns**: Ogni layer ha responsabilità ben definite
✅ **Testability**: Domain logic isolata dall'infrastruttura
✅ **Observability**: Trace e log correlati automaticamente
✅ **Scalability**: Servizi indipendenti scalabili separatamente
✅ **Resilience**: Gestione errori quando Kitchen non è disponibile
✅ **Maintainability**: Architettura esagonale facilita modifiche
