# Clean Architecture

## Cos'è?

La **Clean Architecture**, proposta da Robert C. Martin (Uncle Bob), organizza il codice in **layer concentrici** dove le dipendenze puntano sempre verso il centro.

## Concetti Chiave

### 🎯 Cerchi Concentrici
L'architettura è organizzata in cerchi, dal più interno al più esterno:

1. **Entities** (centro): Logica di business fondamentale
2. **Use Cases**: Logica applicativa specifica
3. **Interface Adapters**: Convertitori di dati
4. **Frameworks & Drivers**: Dettagli tecnici esterni

### 📏 Dependency Rule
**Regola fondamentale**: Le dipendenze puntano sempre VERSO L'INTERNO.
- I layer interni NON conoscono i layer esterni
- I layer esterni dipendono dai layer interni

## Struttura del Progetto

```
CleanArchitecture.Domain/              # ⭐ CENTRO - Entities
├── Entities/
│   └── TodoTask.cs                    # Logica di business
└── Repositories/
    └── ITaskRepository.cs             # Interfaccia (definita qui!)

CleanArchitecture.UseCases/            # ⭐ Use Cases
├── CreateTask/
│   ├── CreateTaskRequest.cs
│   ├── CreateTaskResponse.cs
│   └── CreateTaskUseCase.cs
├── CompleteTask/
│   └── CompleteTaskUseCase.cs
└── GetAllTasks/
    └── GetAllTasksUseCase.cs

CleanArchitecture.Adapters/            # ⭐ Interface Adapters
└── Persistence/
    └── InMemoryTaskRepository.cs      # Implementazione repository

CleanArchitecture.WebApi/              # ⭐ Frameworks & Drivers
└── Program.cs                         # Entry point & DI
```

## Differenze con l'Architettura Esagonale

| Aspetto | Clean Architecture | Hexagonal Architecture |
|---------|-------------------|------------------------|
| **Focus** | Layer concentrici | Porte e Adapter |
| **Organizzazione** | Use Cases espliciti | Servizi applicativi |
| **Terminologia** | Entities, Use Cases | Domain, Ports, Adapters |
| **Obiettivo** | Stesso: indipendenza dal framework e testabilità |

## Regole di Dipendenza

```
         ┌─────────────────────┐
         │  Frameworks & Web   │  ← Layer più esterno
         │    (WebApi)         │
         └──────────┬──────────┘
                    │
         ┌──────────▼──────────┐
         │  Interface Adapters │
         │    (Adapters)       │
         └──────────┬──────────┘
                    │
         ┌──────────▼──────────┐
         │     Use Cases       │  ← Logica applicativa
         └──────────┬──────────┘
                    │
         ┌──────────▼──────────┐
         │      Entities       │  ← Logica di business
         │      (Domain)       │      (Centro!)
         └─────────────────────┘
```

## Vantaggi

✅ **Indipendenza dal Framework**: Il business non dipende da ASP.NET, Entity Framework, ecc.
✅ **Testabile**: Ogni layer può essere testato indipendentemente
✅ **Indipendenza dalla UI**: Puoi cambiare la UI senza toccare il business
✅ **Indipendenza dal Database**: Puoi cambiare il DB senza toccare il business
✅ **Indipendenza da agenti esterni**: Il business non sa nulla del mondo esterno

## Principi SOLID Applicati

- **SRP**: Ogni Use Case ha una singola responsabilità
- **OCP**: Estendibile tramite nuovi Use Cases senza modificare esistenti
- **LSP**: Gli adapter possono essere sostituiti
- **ISP**: Interfacce specifiche per ogni need
- **DIP**: Tutti dipendono da astrazioni (interfacce)
