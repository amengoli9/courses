// ===================================================================
// BLOCCO 4 — Code Smells: Dare un Nome ai Problemi
// Smell 4: Large Class (God Class)
//
// ❌ GOD CLASS — Fa tutto ciò che riguarda un utente.
// Indicatori: 20+ metodi, nome generico "Manager",
// responsabilità multiple (auth, CRUD, permessi, notifiche, report).
// Due developer che lavorano su funzionalità diverse toccano lo
// stesso file = conflitti di merge garantiti.
// ===================================================================

using System.Net.Mail;
using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block4_CodeSmells;

public class UserManager
{
    private readonly string _connectionString;
    private readonly SmtpClient _smtpClient;

    // ---- Autenticazione ----
    public bool Login(string username, string password) { /* ... */ return true; }
    public void Logout(int userId) { /* ... */ }
    public string GeneraToken(int userId) { /* ... */ return ""; }
    public bool ValidaToken(string token) { /* ... */ return true; }
    public void ResetPassword(string email) { /* ... */ }
    public void CambiaPassword(int userId, string old, string nuova) { /* ... */ }
    public bool VerificaFortePassword(string password) { /* ... */ return true; }

    // ---- CRUD ----
    public void CreaUtente(string nome, string email) { /* ... */ }
    public User GetUtente(int id) { /* ... */ return null; }
    public List<User> GetTuttiUtenti() { /* ... */ return new(); }
    public void AggiornaUtente(User user) { /* ... */ }
    public void EliminaUtente(int id) { /* ... */ }
    public List<User> CercaUtenti(string query) { /* ... */ return new(); }
    public void ImportaUtentiDaCsv(string path) { /* ... */ }
    public void EsportaUtentiInCsv(string path) { /* ... */ }

    // ---- Permessi ----
    public bool HaPermesso(int userId, string permesso) { /* ... */ return true; }
    public void AssegnaRuolo(int userId, string ruolo) { /* ... */ }
    public void RimuoviRuolo(int userId, string ruolo) { /* ... */ }
    public List<string> GetPermessi(int userId) { /* ... */ return new(); }

    // ---- Notifiche ----
    public void InviaEmailBenvenuto(int userId) { /* ... */ }
    public void InviaEmailResetPassword(string email, string token) { /* ... */ }
    public void InviaNotificaPush(int userId, string messaggio) { /* ... */ }
    public void InviaRiepilogoSettimanale(int userId) { /* ... */ }

    // ---- Report ----
    public object GetStatisticheUtente(int userId) { /* ... */ return null; }
    public object GetReportAttivita(DateTime da, DateTime a) { /* ... */ return null; }

    // ... e altri 30 metodi
}
