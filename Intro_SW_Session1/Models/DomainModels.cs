// ===================================================================
// Modelli di dominio condivisi — Usati negli esempi di tutti i blocchi
// ===================================================================

namespace Intro_SW_Session1.Models;

// --- Ordini ---

public class Ordine
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public decimal Totale { get; set; }
    public Cliente Cliente { get; set; }
    public List<Prodotto> Prodotti { get; set; }
    public DateTime DataConsegna { get; set; }
}

public class Prodotto
{
    public string Nome { get; set; }
    public double Prezzo { get; set; }
    public int Quantita { get; set; }
    public bool IsOmaggio { get; set; }
}

public class Cliente
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string StripeCustomerId { get; set; }
    public bool IsApprovato { get; set; }
}

public class PagamentoResult
{
    public bool Successo { get; set; }
    public string TransactionId { get; set; }
}

// --- Dipendenti ---

public class Impiegato
{
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public string Email { get; set; }
    public int Eta { get; set; }
    public string Dipartimento { get; set; }
    public DateTime DataAssunzione { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
}

// --- Vendite ---

public class Vendita
{
    public int Id { get; set; }
    public string Prodotto { get; set; }
    public decimal Importo { get; set; }
    public DateTime Data { get; set; }
}

// --- Import ---

public class ImportResult
{
    public int TotaleRighe { get; set; }
    public int RigheImportate { get; set; }
    public int RigheScartate { get; set; }
    public int RigheConErroriFormato { get; set; }
    public List<string> Errori { get; set; }
    public List<Dictionary<string, string>> Dati { get; set; }
}

public class ClienteCSV
{
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }
}

// --- Minesweeper ---

public class Cella
{
    public int Riga { get; set; }
    public int Colonna { get; set; }
    public bool IsSegnata { get; set; }
    public bool IsMinata { get; set; }
}

// --- Statistiche ---

public class StatisticheVendite
{
    public decimal Totale { get; set; }
    public decimal Media { get; set; }
    public decimal Massimo { get; set; }
    public decimal Minimo { get; set; }
    public Dictionary<string, decimal> PerProdotto { get; set; }
}

// --- Validation ---

public class ValidationResult
{
    public bool IsValido { get; private set; }
    public string Errore { get; private set; }
    public string ValorePulito { get; private set; }

    public static ValidationResult Ok(string valore = null) =>
        new() { IsValido = true, ValorePulito = valore };

    public static ValidationResult Errore(string messaggio) =>
        new() { IsValido = false, Errore = messaggio };
}

// --- Enums ---

public enum TipoCalcolo
{
    Addizione,
    Sottrazione,
    Moltiplicazione
}

public enum TipoPiano
{
    Standard,
    Premium,
    Enterprise
}
