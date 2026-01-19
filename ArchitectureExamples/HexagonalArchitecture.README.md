# Architettura Esagonale (Hexagonal Architecture / Ports & Adapters)

## Cos'è?

L'Architettura Esagonale, anche chiamata **Ports & Adapters**, è un pattern architetturale che mette il **dominio al centro** dell'applicazione, isolandolo completamente dai dettagli tecnici esterni.

**Caratteristica distintiva**: L'architettura esagonale ha **solo 3 layer** (Domain, Infrastructure, Api), mentre la Clean Architecture ne ha 4 (Domain, Application, Infrastructure, Presentation). Questo la rende più semplice e diretta.

## Concetti Chiave

### 🔷 Esagono (Hexagon)
Il nucleo dell'applicazione che contiene:
- **Domain**: Entità di business, logica di dominio e definizione delle Porte (Ports)

### 🔌 Porte (Ports)
Interfacce **definite dal dominio** che specificano COSA serve all'applicazione:
- `IOrderRepository`: porta per la persistenza
- `INotificationService`: porta per le notifiche

### 🔧 Adapter
Implementazioni **concrete** che specificano COME funzionano le porte:
- `InMemoryOrderRepository`: adapter per persistenza in memoria
- `ConsoleNotificationService`: adapter per notifiche console

## Struttura del Progetto

```
HexagonalArchitecture.Domain/          # NUCLEO - Non dipende da nulla
├── Order.cs                            # Entità del dominio con logica di business
└── Ports/                              # Interfacce definite dal dominio
    ├── IOrderRepository.cs
    └── INotificationService.cs

HexagonalArchitecture.Infrastructure/  # ADAPTER - Dettagli tecnici
└── Adapters/
    ├── InMemoryOrderRepository.cs      # Implementazione concreta della porta
    └── ConsoleNotificationService.cs   # Implementazione concreta della porta

HexagonalArchitecture.Api/             # DRIVER ADAPTER - Entry point
└── Program.cs                          # Wiring e coordinamento dei casi d'uso
```

## Vantaggi

✅ **Testabilità**: Il dominio può essere testato senza dipendenze esterne
✅ **Flessibilità**: Gli adapter possono essere sostituiti facilmente
✅ **Indipendenza dal framework**: Il dominio non dipende da tecnologie specifiche
✅ **Manutenibilità**: La logica di business è isolata e protetta

## Regole di Dipendenza

```
Infrastructure → Domain
     ↑              ↑
     └──────────────┴──── Api
```

- Il **Domain** non dipende da nessuno (è il centro dell'esagono)
- L'**Infrastructure** implementa le porte del Domain (driven adapters)
- L'**Api** usa le porte del Domain e gli adapter dell'Infrastructure (driver adapter)
- **Nota**: A differenza della Clean Architecture, non c'è un layer Application separato - i casi d'uso sono coordinati direttamente dall'Api usando le porte del Domain
