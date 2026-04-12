// ===================================================================
// BLOCCO 3 — Clean Code: I Principi di Robert C. Martin
// Sezione 3.1 — Naming: Il principio più importante
//
// ✅ VERSIONE BUONA — 'Proc' diventa un metodo con un nome
// che dice cosa fa. Il metodo RegistraNuovoImpiegato è un indice:
// leggendolo capite il flusso senza leggere 50 righe.
// "Leggere il codice come un giornale" — Uncle Bob
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block3_CleanCode;

public class RegistrazioneImpiegato
{
    public void RegistraNuovoImpiegato(
        string nome,
        string cognome,
        string email,
        int eta,
        string dipartimento)
    {
        ValidaDatiImpiegato(nome, cognome, email, eta);
        var impiegato = CreaImpiegato(nome, cognome, email, eta, dipartimento);
        SalvaImpiegato(impiegato);
        InviaEmailBenvenuto(impiegato);
    }

    private void ValidaDatiImpiegato(
        string nome, string cognome, string email, int eta)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Il nome è obbligatorio.", nameof(nome));

        if (string.IsNullOrWhiteSpace(cognome))
            throw new ArgumentException("Il cognome è obbligatorio.", nameof(cognome));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            throw new ArgumentException("Email non valida.", nameof(email));

        if (eta < 18 || eta > 70)
            throw new ArgumentOutOfRangeException(
                nameof(eta), "L'età deve essere compresa tra 18 e 70.");
    }

    private Impiegato CreaImpiegato(
        string nome, string cognome, string email,
        int eta, string dipartimento)
    {
        return new Impiegato
        {
            Nome = nome,
            Cognome = cognome,
            Email = email,
            Eta = eta,
            Dipartimento = dipartimento,
            DataAssunzione = DateTime.Today
        };
    }

    private void SalvaImpiegato(Impiegato impiegato)
    {
        // Salva nel database
    }

    private void InviaEmailBenvenuto(Impiegato impiegato)
    {
        // Invia email di benvenuto
    }
}
