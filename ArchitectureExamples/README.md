# Esempi di Architetture Software con Fitness Functions

Questo progetto contiene esempi didattici di due architetture software moderne:
- **Architettura Esagonale** (Hexagonal Architecture / Ports & Adapters)
- **Clean Architecture**

Ogni esempio include **test di fitness function** usando **NetArchTest** per verificare che le regole architetturali siano rispettate.

## 📁 Struttura del Progetto

```
ArchitectureExamples/
│
├── 🔷 HEXAGONAL ARCHITECTURE (OrderManagement)
│   ├── HexagonalArchitecture.Domain/          # Nucleo: Entità + Ports
│   ├── HexagonalArchitecture.Application/     # Casi d'uso
│   ├── HexagonalArchitecture.Infrastructure/  # Adapters (implementazioni)
│   ├── HexagonalArchitecture.Api/             # Entry point
│   └── HexagonalArchitecture.README.md
│
├── 🎯 CLEAN ARCHITECTURE (TaskManagement)
│   ├── CleanArchitecture.Domain/              # Entities + Repositories (interfacce)
│   ├── CleanArchitecture.UseCases/            # Use Cases
│   ├── CleanArchitecture.Adapters/            # Interface Adapters
│   ├── CleanArchitecture.WebApi/              # Entry point
│   └── CleanArchitecture.README.md
│
└── 🧪 ARCHITECTURE TESTS (NetArchTest)
    └── ArchitectureTests/
        ├── HexagonalArchitectureTests.cs      # Fitness functions per Hexagonal
        └── CleanArchitectureTests.cs          # Fitness functions per Clean
```

## 🎓 Cosa Imparerai

### 1. Architettura Esagonale
- Come isolare il dominio dai dettagli tecnici
- Cos'è una **Porta** (Port) e un **Adapter**
- Come rendere il codice indipendente dal database, framework, UI
- Esempio: Sistema di gestione ordini

### 2. Clean Architecture
- I layer concentrici di Uncle Bob
- La **Dependency Rule**: dipendenze verso il centro
- Come organizzare Use Cases espliciti
- Esempio: Sistema di gestione task (TODO list)

### 3. Fitness Functions con NetArchTest
- Come scrivere test che verificano l'architettura
- Come prevenire violazioni delle regole architetturali
- Test automatizzati per le dipendenze tra layer

## 🚀 Come Eseguire gli Esempi

### Architettura Esagonale
```bash
cd HexagonalArchitecture.Api
dotnet run
```

### Clean Architecture
```bash
cd CleanArchitecture.WebApi
dotnet run
```

### Eseguire i Test di Architettura
```bash
cd ArchitectureTests
dotnet test
```

## 🧪 Cosa Fanno i Test NetArchTest?

I test verificano automaticamente che:

### Per l'Architettura Esagonale:
✅ Il **Domain** non dipenda da Application, Infrastructure o Api
✅ L'**Application** non dipenda da Infrastructure
✅ Le **Ports** siano interfacce
✅ Gli **Adapters** implementino le Ports
✅ Il Domain non dipenda da librerie esterne

### Per la Clean Architecture:
✅ Il **Domain** non dipenda da altri layer (è il centro!)
✅ Gli **UseCases** dipendano solo dal Domain
✅ Gli **Adapters** dipendano solo dal Domain
✅ I **Repository** nel Domain siano interfacce
✅ Gli Use Cases seguano naming conventions ("UseCase" suffix)
✅ Il Domain non dipenda da framework esterni

## 💡 Fitness Functions: Cosa Sono?

Le **Fitness Functions** sono test automatizzati che verificano aspetti qualitativi del software, come l'architettura.

### Vantaggi:
- ✅ Prevengono violazioni architetturali durante lo sviluppo
- ✅ Documentano le regole architetturali come codice
- ✅ Proteggono il codice da dipendenze indesiderate
- ✅ Facili da eseguire in CI/CD

### Esempio di Fitness Function:
```csharp
[Fact]
public void Domain_Should_Not_Depend_On_Infrastructure()
{
    var result = Types.InAssembly(domainAssembly)
        .That()
        .ResideInNamespace("MyApp.Domain")
        .ShouldNot()
        .HaveDependencyOn("MyApp.Infrastructure")
        .GetResult();

    Assert.True(result.IsSuccessful);
}
```

Se qualcuno aggiunge una dipendenza da Domain a Infrastructure, il test fallisce! 🛑

## 📚 Confronto tra le Due Architetture

| Aspetto | Hexagonal | Clean |
|---------|-----------|-------|
| **Organizzazione** | Porta/Adapter | Layer concentrici |
| **Focus** | Isolamento del dominio tramite ports | Dependency Rule (verso il centro) |
| **Use Cases** | Impliciti (servizi applicativi) | Espliciti (una classe = un use case) |
| **Terminologia** | Domain, Ports, Adapters | Entities, Use Cases, Adapters |
| **Obiettivo** | 🎯 Stesso: Indipendenza, Testabilità, Manutenibilità |

## 🎯 Principi Applicati

Entrambe le architetture applicano i principi SOLID:
- **SRP**: Ogni componente ha una responsabilità unica
- **OCP**: Estendibile senza modificare codice esistente
- **LSP**: Gli adapter sono sostituibili
- **ISP**: Interfacce specifiche e segregate
- **DIP**: Dipendenza da astrazioni, non da implementazioni

## 📖 Risorse Aggiuntive

- **Hexagonal Architecture**: Alistair Cockburn
- **Clean Architecture**: Robert C. Martin (Uncle Bob)
- **NetArchTest**: [GitHub](https://github.com/BenMorris/NetArchTest)

## 🎓 Uso Didattico

Questi esempi sono progettati per essere:
- ✅ **Semplici**: Codice minimo necessario per capire i concetti
- ✅ **Chiari**: Commenti esplicativi in ogni file
- ✅ **Completi**: Tutti i layer rappresentati
- ✅ **Testabili**: Fitness functions incluse

Perfetti per:
- Corsi di architettura software
- Workshop e training
- Studio individuale
- Reference per progetti reali

---

**Buono studio! 🚀**
