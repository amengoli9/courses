/*
 * ============================================================================
 * 2. CONNASCENCE OF TYPE (CoT)
 * ============================================================================
 *
 * DEFINIZIONE: Più componenti devono concordare sul TIPO di dato
 * FORZA: ⭐⭐ DEBOLE-MEDIA
 *
 * ============================================================================
 */

// ============================================================================
// ESEMPIO SEMPLICE PER SLIDE
// ============================================================================

/*
PROBLEMA:
    List<Customer> GetCustomers()  // Client deve usare List

SOLUZIONE:
    IEnumerable<Customer> GetCustomers()  // Client usa interfaccia più generica
*/

// ============================================================================
// ESEMPIO C#
// ============================================================================

namespace ConnascenceCourse.ConnascenceOfType
{
    // ❌ PROBLEMA: Accoppiamento a tipi concreti
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SqlCustomerRepository
    {
        // Ritorna tipo concreto List
        public List<Customer> GetAllCustomers()
        {
            return new List<Customer>
            {
                new Customer { Id = 1, Name = "John" },
                new Customer { Id = 2, Name = "Jane" }
            };
        }

        // Prende tipo concreto Dictionary
        public void UpdateCustomers(Dictionary<int, Customer> customers)
        {
            foreach (var kvp in customers)
            {
                Console.WriteLine($"Updating customer {kvp.Key}");
            }
        }
    }

    public class CustomerService
    {
        private SqlCustomerRepository _repository;

        public void ProcessCustomers()
        {
            // Devo usare List - non posso usare array o IEnumerable
            List<Customer> customers = _repository.GetAllCustomers();

            // Devo creare un Dictionary - non posso usare altro
            Dictionary<int, Customer> dict = new Dictionary<int, Customer>();
            _repository.UpdateCustomers(dict);
        }
    }

    // PROBLEMA: Se cambio List in ImmutableList o Customer[] il client si rompe

    // ✅ SOLUZIONE: Usa interfacce e tipi più generici
    public interface ICustomerRepository
    {
        // IEnumerable invece di List - più flessibile
        IEnumerable<Customer> GetAllCustomers();

        // IReadOnlyDictionary invece di Dictionary - più chiaro sull'intento
        void UpdateCustomers(IReadOnlyDictionary<int, Customer> customers);
    }

    public class SqlCustomerRepositoryRefactored : ICustomerRepository
    {
        public IEnumerable<Customer> GetAllCustomers()
        {
            // Posso ritornare List, Array, ImmutableList, etc.
            return new List<Customer>
            {
                new Customer { Id = 1, Name = "John" },
                new Customer { Id = 2, Name = "Jane" }
            };
        }

        public void UpdateCustomers(IReadOnlyDictionary<int, Customer> customers)
        {
            foreach (var kvp in customers)
            {
                Console.WriteLine($"Updating customer {kvp.Key}");
            }
        }
    }

    public class CustomerServiceRefactored
    {
        private readonly ICustomerRepository _repository;

        public CustomerServiceRefactored(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public void ProcessCustomers()
        {
            // Posso usare var - non sono legato al tipo concreto
            IEnumerable<Customer> customers = _repository.GetAllCustomers();

            // Posso passare qualsiasi IReadOnlyDictionary
            var dict = new Dictionary<int, Customer>();
            _repository.UpdateCustomers(dict);
        }
    }

    // VANTAGGI:
    // - Posso cambiare implementazione senza rompere i client
    // - IEnumerable permette lazy evaluation
    // - IReadOnlyDictionary è più chiaro sull'intento (non modificabile)
}
