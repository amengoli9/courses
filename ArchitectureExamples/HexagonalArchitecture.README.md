# Architettura Esagonale (Hexagonal Architecture / Ports & Adapters)

## Cos'è?

L'Architettura Esagonale, anche chiamata **Ports & Adapters**, è un pattern architetturale che mette il **dominio al centro** dell'applicazione, isolandolo completamente dai dettagli tecnici esterni.

## Concetti Chiave

### 🔷 Esagono (Hexagon)
Il nucleo dell'applicazione che contiene:
- **Domain**: Entità di business e logica di dominio
- **Application**: Casi d'uso e servizi applicativi

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
├── Order.cs                            # Entità del dominio
└── Ports/                              # Interfacce definite dal dominio
    ├── IOrderRepository.cs
    └── INotificationService.cs

HexagonalArchitecture.Application/     # Casi d'uso
└── OrderService.cs                     # Coordina le operazioni

HexagonalArchitecture.Infrastructure/  # ADAPTER - Dettagli tecnici
└── Adapters/
    ├── InMemoryOrderRepository.cs      # Implementazione concreta
    └── ConsoleNotificationService.cs   # Implementazione concreta

HexagonalArchitecture.Api/             # Entry point
└── Program.cs                          # Wiring delle dipendenze
```

## Vantaggi

✅ **Testabilità**: Il dominio può essere testato senza dipendenze esterne
✅ **Flessibilità**: Gli adapter possono essere sostituiti facilmente
✅ **Indipendenza dal framework**: Il dominio non dipende da tecnologie specifiche
✅ **Manutenibilità**: La logica di business è isolata e protetta

## Regole di Dipendenza

```
Infrastructure → Application → Domain
     ↑              ↑
     └──────────────┴──── Api
```

- Il **Domain** non dipende da nessuno
- L'**Application** dipende solo dal Domain
- L'**Infrastructure** implementa le porte del Domain
- L'**Api** conosce tutto e fa il wiring
