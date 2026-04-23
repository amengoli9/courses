# Corso Connascence - 3 Ore

## Obiettivi del Corso
Comprendere i 9 tipi di connascence (accoppiamento) nel software e imparare a identificarli e risolverli per migliorare la qualità del codice.

## Struttura del Corso (3 ore)

### 1. Introduzione (30 minuti)
- **Cos'è la Connascence?** (10 min)
  - Definizione: misura dell'accoppiamento tra componenti software
  - Storia: introdotta da Meilir Page-Jones
  - Perché è importante: manutenibilità, testabilità, scalabilità

- **Le Due Categorie** (10 min)
  - **Connascence Statica**: rilevabile analizzando il codice
  - **Connascence Dinamica**: rilevabile solo durante l'esecuzione

- **Le Tre Proprietà della Connascence** (10 min)
  - **Strength (Forza)**: quanto è difficile da cambiare
  - **Degree (Grado)**: quanti elementi sono accoppiati
  - **Locality (Località)**: quanto sono vicini gli elementi

### 2. Connascence Statica (1 ora 15 minuti)

#### 2.1 Connascence of Name (CoN) - 15 min
- Definizione: più componenti devono concordare sullo stesso nome
- Esempio pratico in C#
- Problema e refactoring
- **Forza**: Debole (la più facile da gestire)

#### 2.2 Connascence of Type (CoT) - 15 min
- Definizione: più componenti devono concordare sul tipo di dato
- Esempio pratico in C#
- Problema e refactoring
- **Forza**: Debole-Media

#### 2.3 Connascence of Meaning (CoM) - 15 min
- Definizione: più componenti devono concordare sul significato di valori specifici
- Esempio pratico in C#
- Problema e refactoring
- **Forza**: Media-Alta

#### 2.4 Connascence of Position (CoP) - 15 min
- Definizione: più componenti devono concordare sull'ordine dei valori
- Esempio pratico in C#
- Problema e refactoring
- **Forza**: Media-Alta

#### 2.5 Connascence of Algorithm (CoA) - 15 min
- Definizione: più componenti devono concordare su un algoritmo specifico
- Esempio pratico in C#
- Problema e refactoring
- **Forza**: Alta

### 3. Connascence Dinamica (1 ora)

#### 3.1 Connascence of Execution (CoE) - 15 min
- Definizione: l'ordine di esecuzione di più componenti è importante
- Esempio pratico in C#
- Problema e refactoring
- **Forza**: Alta

#### 3.2 Connascence of Timing (CoTi) - 15 min
- Definizione: il timing dell'esecuzione di più componenti è importante
- Esempio pratico in C#
- Problema e refactoring
- **Forza**: Molto Alta

#### 3.3 Connascence of Value (CoV) - 15 min
- Definizione: più componenti devono concordare su valori specifici
- Esempio pratico in C#
- Problema e refactoring
- **Forza**: Molto Alta

#### 3.4 Connascence of Identity (CoI) - 15 min
- Definizione: più componenti devono riferirsi allo stesso oggetto
- Esempio pratico in C#
- Problema e refactoring
- **Forza**: Estremamente Alta

### 4. Conclusioni e Best Practices (15 minuti)
- **Regola d'oro**: Minimizzare la connascence
  - Convertire forme forti in forme deboli
  - Ridurre il grado (numero di elementi accoppiati)
  - Aumentare la località (tenere vicino il codice accoppiato)

- **Ordine di preferenza**:
  1. Connascence of Name (migliore)
  2. Connascence of Type
  3. Connascence of Meaning
  4. Connascence of Position
  5. Connascence of Algorithm
  6. Connascence of Execution
  7. Connascence of Timing
  8. Connascence of Value
  9. Connascence of Identity (peggiore)

- **Q&A e discussione**

## Come Usare Questo Repository

Ogni cartella contiene:
- `Problem.cs`: Esempio del problema di connascence
- `Refactored.cs`: Soluzione migliorata
- `README.md`: Spiegazione dettagliata

## Prerequisiti
- Conoscenza base di C#
- Comprensione di OOP
- Familiarità con pattern di design (consigliato ma non obbligatorio)

## Risorse Aggiuntive
- Libro: "Structured Design" di Meilir Page-Jones
- Articolo: "Connascence" su martinfowler.com
- Talk: "Jim Weirich - The Building Blocks of Modularity"
