/*
 * ============================================================================
 * 9. CONNASCENCE OF IDENTITY (CoI)
 * ============================================================================
 *
 * DEFINIZIONE: Più componenti devono riferirsi allo STESSO OGGETTO
 * FORZA: ⭐⭐⭐⭐⭐⭐ ESTREMAMENTE ALTA (la peggiore!)
 *
 * ============================================================================
 */

// ============================================================================
// ESEMPIO SEMPLICE PER SLIDE
// ============================================================================

/*
PROBLEMA:
    var obj1 = new MyClass();
    DoSomething(obj1);  // Modifica obj1
    DoOtherThing(obj1); // Legge obj1
    // DEVONO usare lo STESSO oggetto!

SOLUZIONE:
    var value = new ImmutableClass();
    var newValue = DoSomething(value);  // Ritorna nuovo valore
    DoOtherThing(newValue);
*/

// ============================================================================
// ESEMPIO C#
// ============================================================================

namespace ConnascenceCourse.ConnascenceOfIdentity
{
    // ❌ PROBLEMA: Oggetto mutabile condiviso
    public class UserSession
    {
        public string Username { get; set; }
        public bool IsAuthenticated { get; set; }
        public DateTime LastActivity { get; set; }
    }

    public class AuthService
    {
        public void Login(UserSession session, string username, string password)
        {
            // Modifica l'oggetto condiviso
            session.Username = username;
            session.IsAuthenticated = true;
            session.LastActivity = DateTime.Now;
        }

        public void Logout(UserSession session)
        {
            // Modifica lo stesso oggetto
            session.IsAuthenticated = false;
        }
    }

    public class ActivityTracker
    {
        public void UpdateActivity(UserSession session)
        {
            // Modifica lo stesso oggetto
            session.LastActivity = DateTime.Now;
        }
    }

    public class Example
    {
        public void Test()
        {
            var session = new UserSession(); // Oggetto condiviso

            var authService = new AuthService();
            var tracker = new ActivityTracker();

            // Tutti modificano LO STESSO oggetto
            authService.Login(session, "john", "password");
            tracker.UpdateActivity(session);
            authService.Logout(session);

            // Difficile tracciare chi ha modificato cosa e quando
            // Race conditions in ambiente multi-thread
        }
    }

    // ✅ SOLUZIONE 1: Immutabilità (Value Objects)
    public record UserSessionRefactored(
        string Username,
        bool IsAuthenticated,
        DateTime LastActivity)
    {
        // Metodi che ritornano nuove istanze
        public UserSessionRefactored WithLogin(string username)
        {
            return this with
            {
                Username = username,
                IsAuthenticated = true,
                LastActivity = DateTime.Now
            };
        }

        public UserSessionRefactored WithLogout()
        {
            return this with { IsAuthenticated = false };
        }

        public UserSessionRefactored WithActivityUpdate()
        {
            return this with { LastActivity = DateTime.Now };
        }
    }

    public class AuthServiceRefactored
    {
        public UserSessionRefactored Login(UserSessionRefactored session, string username, string password)
        {
            // Ritorna NUOVO oggetto invece di modificare
            return session.WithLogin(username);
        }

        public UserSessionRefactored Logout(UserSessionRefactored session)
        {
            return session.WithLogout();
        }
    }

    public class ExampleRefactored1
    {
        public void Test()
        {
            var session = new UserSessionRefactored("", false, DateTime.Now);
            var authService = new AuthServiceRefactored();

            // Ogni operazione ritorna una nuova versione
            var loggedInSession = authService.Login(session, "john", "password");
            var updatedSession = loggedInSession.WithActivityUpdate();
            var loggedOutSession = authService.Logout(updatedSession);

            // Nessuna condivisione di identità!
        }
    }

    // ✅ SOLUZIONE 2: Copy-on-Write
    public class Configuration
    {
        public string DatabaseConnection { get; private set; }
        public int MaxRetries { get; private set; }

        public Configuration(string dbConnection, int maxRetries)
        {
            DatabaseConnection = dbConnection;
            MaxRetries = maxRetries;
        }

        // Ritorna copia modificata
        public Configuration WithDatabaseConnection(string connection)
        {
            return new Configuration(connection, MaxRetries);
        }

        public Configuration WithMaxRetries(int retries)
        {
            return new Configuration(DatabaseConnection, retries);
        }
    }

    // ✅ SOLUZIONE 3: Message Passing invece di oggetto condiviso
    public class UserSessionManager
    {
        private readonly Dictionary<string, UserSessionRefactored> _sessions = new();
        private readonly object _lock = new object();

        public void UpdateSession(string sessionId, Func<UserSessionRefactored, UserSessionRefactored> update)
        {
            lock (_lock)
            {
                if (_sessions.TryGetValue(sessionId, out var currentSession))
                {
                    // Applica l'update e salva la nuova versione
                    _sessions[sessionId] = update(currentSession);
                }
            }
        }

        public UserSessionRefactored GetSession(string sessionId)
        {
            lock (_lock)
            {
                _sessions.TryGetValue(sessionId, out var session);
                return session; // Ritorna copia (record è immutabile)
            }
        }
    }

    public class ExampleRefactored3
    {
        public void Test()
        {
            var manager = new UserSessionManager();

            // Nessun oggetto condiviso direttamente
            manager.UpdateSession("user123", session => session.WithLogin("john"));
            manager.UpdateSession("user123", session => session.WithActivityUpdate());

            var currentSession = manager.GetSession("user123");
        }
    }

    // Esempio: Event Sourcing (alternativa radicale)
    public record UserEvent
    {
        public record LoggedIn(string Username, DateTime Timestamp) : UserEvent;
        public record LoggedOut(DateTime Timestamp) : UserEvent;
        public record ActivityUpdated(DateTime Timestamp) : UserEvent;
    }

    public class UserSessionEventSourcing
    {
        private readonly List<UserEvent> _events = new();

        public void Apply(UserEvent @event)
        {
            _events.Add(@event);
        }

        // Ricostruisce lo stato dagli eventi
        public UserSessionRefactored GetCurrentState()
        {
            var state = new UserSessionRefactored("", false, DateTime.MinValue);

            foreach (var @event in _events)
            {
                state = @event switch
                {
                    UserEvent.LoggedIn e => state with { Username = e.Username, IsAuthenticated = true, LastActivity = e.Timestamp },
                    UserEvent.LoggedOut e => state with { IsAuthenticated = false },
                    UserEvent.ActivityUpdated e => state with { LastActivity = e.Timestamp },
                    _ => state
                };
            }

            return state;
        }
    }

    // VANTAGGI:
    // - Immutabilità: no shared mutable state
    // - Copy-on-Write: modifiche sicure
    // - Message Passing: sincronizzazione controllata
    // - Event Sourcing: storico completo, ricostruzione dello stato
}
