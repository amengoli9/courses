// ===================================================================
// BLOCCO 4 — Code Smells: Dare un Nome ai Problemi
// Smell 4: Large Class (God Class)
//
// ✅ CLASSI SEPARATE — Ognuna ha una responsabilità chiara.
// Se c'è un bug nell'autenticazione, guardo ServizioAutenticazione.
// Se le notifiche non partono, guardo ServizioNotifiche.
// Niente conflitti di merge.
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block4_CodeSmells;

// Solo autenticazione
public class ServizioAutenticazione
{
    public bool Login(string username, string password) { /* ... */ return true; }
    public void Logout(int userId) { /* ... */ }
    public string GeneraToken(int userId) { /* ... */ return ""; }
    public bool ValidaToken(string token) { /* ... */ return true; }
}

// Solo gestione password
public class ServizioPassword
{
    public void ResetPassword(string email) { /* ... */ }
    public void CambiaPassword(int userId, string vecchia, string nuova) { /* ... */ }
    public bool VerificaRobustezza(string password) { /* ... */ return true; }
}

// Solo CRUD utenti
public class RepositoryUtenti
{
    public void Crea(User user) { /* ... */ }
    public User GetById(int id) { /* ... */ return null; }
    public List<User> GetTutti() { /* ... */ return new(); }
    public void Aggiorna(User user) { /* ... */ }
    public void Elimina(int id) { /* ... */ }
    public List<User> Cerca(string query) { /* ... */ return new(); }
}

// Solo import/export
public class ImportExportUtenti
{
    public void ImportaDaCsv(string path) { /* ... */ }
    public void EsportaInCsv(string path) { /* ... */ }
}

// Solo permessi
public class ServizioAutorizzazione
{
    public bool HaPermesso(int userId, string permesso) { /* ... */ return true; }
    public void AssegnaRuolo(int userId, string ruolo) { /* ... */ }
    public void RimuoviRuolo(int userId, string ruolo) { /* ... */ }
}

// Solo notifiche
public class ServizioNotifiche
{
    public void InviaEmailBenvenuto(User user) { /* ... */ }
    public void InviaEmailResetPassword(string email, string token) { /* ... */ }
    public void InviaNotificaPush(int userId, string messaggio) { /* ... */ }
}
