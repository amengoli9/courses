// ===================================================================
// BLOCCO 4 — Code Smells: Dare un Nome ai Problemi
// Smell 3: Duplicazione (DRY — Don't Repeat Yourself)
//
// ✅ SENZA DUPLICAZIONE — La logica di validazione è in un posto solo.
// Il comportamento diverso (eccezione vs skip) è nel chiamante:
// il chiamante sa se vuole fallire o proseguire.
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block4_CodeSmells;

// --- Validatore riusabile ---

public class ValidatoreContatto
{
    public ValidationResult ValidaEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return ValidationResult.Errore("L'email è obbligatoria.");
        if (!email.Contains("@"))
            return ValidationResult.Errore("L'email deve contenere @.");
        if (!email.Contains("."))
            return ValidationResult.Errore(
                "L'email deve contenere un dominio valido.");
        if (email.Length > 254)
            return ValidationResult.Errore("L'email è troppo lunga.");

        return ValidationResult.Ok(email.Trim().ToLower());
    }

    public ValidationResult ValidaTelefono(string telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono))
            return ValidationResult.Ok(telefono); // il telefono è opzionale

        var pulito = telefono
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("/", "");

        if (pulito.Length < 8 || pulito.Length > 15)
            return ValidationResult.Errore(
                "Il numero di telefono non è valido.");

        if (!pulito.All(c => char.IsDigit(c) || c == '+'))
            return ValidationResult.Errore(
                "Il telefono può contenere solo cifre e +.");

        return ValidationResult.Ok(pulito);
    }
}

// --- Gestore che usa il validatore ---

public class GestoreClientiGood
{
    private readonly ValidatoreContatto _validatore;

    public GestoreClientiGood(ValidatoreContatto validatore)
    {
        _validatore = validatore;
    }

    public void CreaCliente(string nome, string cognome,
                            string email, string telefono)
    {
        var emailResult = _validatore.ValidaEmail(email);
        if (!emailResult.IsValido)
            throw new ArgumentException(emailResult.Errore);

        var telResult = _validatore.ValidaTelefono(telefono);
        if (!telResult.IsValido)
            throw new ArgumentException(telResult.Errore);

        // ... crea il cliente con emailResult.ValorePulito
    }

    public void ImportaClienti(List<ClienteCSV> clientiCsv)
    {
        foreach (var csv in clientiCsv)
        {
            var emailResult = _validatore.ValidaEmail(csv.Email);
            if (!emailResult.IsValido)
                continue; // Skip: durante l'import, skippiamo i non validi

            var telResult = _validatore.ValidaTelefono(csv.Telefono);
            if (!telResult.IsValido)
                continue;

            // ... importa con emailResult.ValorePulito, telResult.ValorePulito
        }
    }
}
