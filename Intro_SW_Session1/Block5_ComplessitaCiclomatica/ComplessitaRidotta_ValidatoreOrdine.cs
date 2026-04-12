// ===================================================================
// BLOCCO 5 — Complessità Ciclomatica e Metriche
// Sezione 5.2 — Come ridurre la complessità ciclomatica
//
// ✅ Lo stesso ValidatoreOrdine con complessità ridotta.
// Tecniche usate: Extract Method + Guard Clauses
//
// La complessità totale del codice è la stessa — le regole sono identiche.
// Ma nessun singolo metodo ha complessità superiore a 4.
// Ogni metodo è testabile in isolamento con pochi test focalizzati.
//
//   Valida()           → CC = 3
//   ValidaStruttura()  → CC = 2
//   ValidaProdotti()   → CC = 4
//   ValidaImporti()    → CC = 2
//   ValidaDate()       → CC = 3
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block5_ComplessitaCiclomatica;

public class ValidatoreOrdineRidotto
{
    // Metodo principale — CC = 3 (1 base + 1 if + 1 if)
    public ValidationResult Valida(Ordine ordine)
    {
        if (ordine == null)
            return ValidationResult.Errore("Ordine nullo");

        var risultato = ValidaStruttura(ordine);
        if (!risultato.IsValido) return risultato;

        risultato = ValidaProdotti(ordine.Prodotti);
        if (!risultato.IsValido) return risultato;

        risultato = ValidaImporti(ordine);
        if (!risultato.IsValido) return risultato;

        return ValidaDate(ordine);
    }

    // CC = 2
    private ValidationResult ValidaStruttura(Ordine ordine)
    {
        if (ordine.Prodotti == null || ordine.Prodotti.Count == 0)
            return ValidationResult.Errore("Nessun prodotto");

        if (ordine.Cliente == null)
            return ValidationResult.Errore("Cliente non specificato");

        return ValidationResult.Ok();
    }

    // CC = 4
    private ValidationResult ValidaProdotti(List<Prodotto> prodotti)
    {
        foreach (var p in prodotti)
        {
            if (p.Quantita <= 0)
                return ValidationResult.Errore(
                    $"Quantità non valida per {p.Nome}");

            if (p.Prezzo < 0)
                return ValidationResult.Errore(
                    $"Prezzo negativo per {p.Nome}");

            if (p.Prezzo == 0 && !p.IsOmaggio)
                return ValidationResult.Errore(
                    $"Prezzo zero per {p.Nome} non marcato come omaggio");
        }
        return ValidationResult.Ok();
    }

    // CC = 2
    private ValidationResult ValidaImporti(Ordine ordine)
    {
        var totale = ordine.Prodotti.Sum(p => p.Prezzo * p.Quantita);

        if (totale > 50000m && !ordine.Cliente.IsApprovato)
            return ValidationResult.Errore(
                "Ordine sopra i 50.000€ richiede approvazione");

        return ValidationResult.Ok();
    }

    // CC = 3
    private ValidationResult ValidaDate(Ordine ordine)
    {
        if (ordine.DataConsegna < DateTime.Today)
            return ValidationResult.Errore(
                "Data di consegna nel passato");

        if (ordine.DataConsegna > DateTime.Today.AddYears(1))
            return ValidationResult.Errore(
                "Data di consegna troppo nel futuro");

        return ValidationResult.Ok();
    }
}
