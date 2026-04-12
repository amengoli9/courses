// ===================================================================
// BLOCCO 4 — Code Smells: Dare un Nome ai Problemi
// Smell 3: Duplicazione (DRY — Don't Repeat Yourself)
//
// ❌ DUPLICAZIONE — La stessa logica di validazione copiata 3 volte.
// La copia 3 (import) ha una differenza sottile: skip invece di
// eccezione. Bug o scelta deliberata? Chi lo sa?
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block4_CodeSmells;

public class GestoreClientiBad
{
    public void CreaCliente(string nome, string cognome, string email,
                            string telefono, string codiceFiscale)
    {
        // Validazione email — copia 1
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("L'email è obbligatoria.");
        if (!email.Contains("@"))
            throw new ArgumentException("L'email deve contenere @.");
        if (!email.Contains("."))
            throw new ArgumentException("L'email deve contenere un dominio valido.");
        if (email.Length > 254)
            throw new ArgumentException("L'email è troppo lunga.");
        email = email.Trim().ToLower();

        // Validazione telefono — copia 1
        if (!string.IsNullOrWhiteSpace(telefono))
        {
            telefono = telefono.Replace(" ", "").Replace("-", "").Replace("/", "");
            if (telefono.Length < 8 || telefono.Length > 15)
                throw new ArgumentException("Il numero di telefono non è valido.");
            if (!telefono.All(c => char.IsDigit(c) || c == '+'))
                throw new ArgumentException(
                    "Il telefono può contenere solo cifre e +.");
        }

        // ... crea il cliente
    }

    public void AggiornaCliente(int clienteId, string nome, string cognome,
                                string email, string telefono)
    {
        // Validazione email — copia 2 (IDENTICA alla 1)
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("L'email è obbligatoria.");
        if (!email.Contains("@"))
            throw new ArgumentException("L'email deve contenere @.");
        if (!email.Contains("."))
            throw new ArgumentException("L'email deve contenere un dominio valido.");
        if (email.Length > 254)
            throw new ArgumentException("L'email è troppo lunga.");
        email = email.Trim().ToLower();

        // Validazione telefono — copia 2 (IDENTICA alla 1)
        if (!string.IsNullOrWhiteSpace(telefono))
        {
            telefono = telefono.Replace(" ", "").Replace("-", "").Replace("/", "");
            if (telefono.Length < 8 || telefono.Length > 15)
                throw new ArgumentException("Il numero di telefono non è valido.");
            if (!telefono.All(c => char.IsDigit(c) || c == '+'))
                throw new ArgumentException(
                    "Il telefono può contenere solo cifre e +.");
        }

        // ... aggiorna il cliente
    }

    public void ImportaClienti(List<ClienteCSV> clientiCsv)
    {
        foreach (var csv in clientiCsv)
        {
            // Validazione email — copia 3 (quasi identica, con una differenza!)
            if (string.IsNullOrWhiteSpace(csv.Email))
                continue; // <-- QUI È DIVERSO! Skip invece di eccezione.
                          //     Bug o scelta? Chi lo sa?
            if (!csv.Email.Contains("@"))
                continue;
            if (!csv.Email.Contains("."))
                continue;
            if (csv.Email.Length > 254)
                continue;
            var emailPulita = csv.Email.Trim().ToLower();

            // Validazione telefono — copia 3 (IDENTICA)
            var tel = csv.Telefono;
            if (!string.IsNullOrWhiteSpace(tel))
            {
                tel = tel.Replace(" ", "").Replace("-", "").Replace("/", "");
                if (tel.Length < 8 || tel.Length > 15)
                    continue;
                if (!tel.All(c => char.IsDigit(c) || c == '+'))
                    continue;
            }

            // ... importa il cliente
        }
    }
}
