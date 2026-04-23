/*
 * ============================================================================
 * 6. CONNASCENCE OF EXECUTION (CoE)
 * ============================================================================
 *
 * DEFINIZIONE: L'ORDINE DI ESECUZIONE di più operazioni è importante
 * FORZA: ⭐⭐⭐⭐ ALTA (pericolosa!)
 *
 * ============================================================================
 */

// ============================================================================
// ESEMPIO SEMPLICE PER SLIDE
// ============================================================================

/*
PROBLEMA:
    connection.Open();     // DEVE essere chiamato prima
    connection.Execute();  // DEVE essere chiamato dopo Open
    connection.Close();    // DEVE essere chiamato alla fine

SOLUZIONE:
    using (var connection = factory.CreateConnection())
    {
        connection.Execute();  // Open/Close automatici
    }
*/

// ============================================================================
// ESEMPIO C#
// ============================================================================

namespace ConnascenceCourse.ConnascenceOfExecution
{
    // ❌ PROBLEMA: Sequenza di chiamate obbligatoria
    public class DatabaseConnection
    {
        private bool _isOpen = false;

        public void Open()
        {
            _isOpen = true;
            Console.WriteLine("Connection opened");
        }

        public void ExecuteQuery(string sql)
        {
            if (!_isOpen)
            {
                throw new InvalidOperationException("Connection must be opened first!");
            }
            Console.WriteLine($"Executing: {sql}");
        }

        public void Close()
        {
            _isOpen = false;
            Console.WriteLine("Connection closed");
        }
    }

    public class UserRepository
    {
        public void GetUsers()
        {
            var connection = new DatabaseConnection();

            // DEVO ricordare l'ordine corretto!
            connection.Open();              // 1. Prima Open
            connection.ExecuteQuery("SELECT * FROM Users"); // 2. Poi Execute
            connection.Close();             // 3. Infine Close

            // Cosa succede se dimentico Open?
            // Cosa succede se dimentico Close? (leak di risorse!)
        }
    }

    // ✅ SOLUZIONE 1: Template Method Pattern
    public abstract class DatabaseOperation
    {
        // Template method che definisce la sequenza
        public void Execute()
        {
            Open();
            try
            {
                DoExecute(); // Hook per subclass
            }
            finally
            {
                Close();     // Garantito anche in caso di errore
            }
        }

        private void Open()
        {
            Console.WriteLine("Connection opened");
        }

        protected abstract void DoExecute();

        private void Close()
        {
            Console.WriteLine("Connection closed");
        }
    }

    public class GetUsersOperation : DatabaseOperation
    {
        protected override void DoExecute()
        {
            Console.WriteLine("Executing: SELECT * FROM Users");
        }
    }

    public class Example1
    {
        public void Test()
        {
            var operation = new GetUsersOperation();
            operation.Execute(); // Sequenza corretta garantita!
        }
    }

    // ✅ SOLUZIONE 2: IDisposable pattern
    public class DatabaseConnectionRefactored : IDisposable
    {
        public DatabaseConnectionRefactored()
        {
            Console.WriteLine("Connection opened");
        }

        public void ExecuteQuery(string sql)
        {
            Console.WriteLine($"Executing: {sql}");
        }

        public void Dispose()
        {
            Console.WriteLine("Connection closed");
        }
    }

    public class UserRepositoryRefactored
    {
        public void GetUsers()
        {
            // using garantisce Open (costruttore) e Close (Dispose)
            using (var connection = new DatabaseConnectionRefactored())
            {
                connection.ExecuteQuery("SELECT * FROM Users");
            } // Close automatico qui!
        }
    }

    // ✅ SOLUZIONE 3: Fluent API con Builder
    public class QueryBuilder
    {
        private string _sql;
        private DatabaseConnectionRefactored _connection;

        public QueryBuilder From(string table)
        {
            _sql = $"SELECT * FROM {table}";
            return this;
        }

        public List<string> Execute()
        {
            // Sequenza corretta incapsulata
            using (var connection = new DatabaseConnectionRefactored())
            {
                connection.ExecuteQuery(_sql);
                return new List<string> { "User1", "User2" };
            }
        }
    }

    public class Example3
    {
        public void Test()
        {
            // Non devo preoccuparmi della sequenza!
            var users = new QueryBuilder()
                .From("Users")
                .Execute();
        }
    }

    // VANTAGGI:
    // - Template Method: sequenza codificata in un solo posto
    // - IDisposable: using garantisce cleanup
    // - Fluent API: nasconde la complessità della sequenza
    // - Impossibile dimenticare passi
}
