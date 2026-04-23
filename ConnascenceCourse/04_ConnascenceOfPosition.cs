/*
 * ============================================================================
 * 4. CONNASCENCE OF POSITION (CoP)
 * ============================================================================
 *
 * DEFINIZIONE: Più componenti devono concordare sull'ORDINE degli elementi
 * FORZA: ⭐⭐⭐ MEDIA-ALTA (pericolosa!)
 *
 * ============================================================================
 */

// ============================================================================
// ESEMPIO SEMPLICE PER SLIDE
// ============================================================================

/*
PROBLEMA:
    CreateUser("John", "Doe", "john@email.com", "Italy", "Rome")
    // Qual è il cognome? Quale la città?

SOLUZIONE:
    CreateUser(
        firstName: "John",
        lastName: "Doe",
        email: "john@email.com",
        country: "Italy",
        city: "Rome"
    )
*/

// ============================================================================
// ESEMPIO C#
// ============================================================================

namespace ConnascenceCourse.ConnascenceOfPosition
{
    // ❌ PROBLEMA: Troppi parametri posizionali
    public class UserService
    {
        // 6 parametri - facile confondersi!
        public void CreateUser(
            string firstName,
            string lastName,
            string email,
            string password,
            string country,
            string city)
        {
            Console.WriteLine($"Creating user: {firstName} {lastName} from {city}, {country}");
        }
    }

    public class Example
    {
        public void Test()
        {
            var service = new UserService();

            // Qual è l'ordine corretto?? Facile sbagliare!
            service.CreateUser("John", "Doe", "john@email.com", "password123", "Italy", "Rome");
            
            // Cosa succede se scambio country e city?
            service.CreateUser("Jane", "Doe", "jane@email.com", "password456", "Milan", "Italy"); // ❌ ERRORE!
        }
    }

    // ✅ SOLUZIONE 1: Named parameters
    public class UserServiceRefactored1
    {
        public void CreateUser(
            string firstName,
            string lastName,
            string email,
            string password,
            string country,
            string city)
        {
            Console.WriteLine($"Creating user: {firstName} {lastName} from {city}, {country}");
        }
    }

    public class Example1
    {
        public void Test()
        {
            var service = new UserServiceRefactored1();

            // Named parameters - chiaro e impossibile sbagliare ordine!
            service.CreateUser(
                firstName: "John",
                lastName: "Doe",
                email: "john@email.com",
                password: "password123",
                country: "Italy",
                city: "Rome"
            );
        }
    }

    // ✅ SOLUZIONE 2: Parameter Object
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }

    public class UserServiceRefactored2
    {
        // Un solo parametro invece di 6!
        public void CreateUser(CreateUserRequest request)
        {
            Console.WriteLine($"Creating user: {request.FirstName} {request.LastName} from {request.City}, {request.Country}");
        }
    }

    public class Example2
    {
        public void Test()
        {
            var service = new UserServiceRefactored2();

            // Object initializer - ordine non importa!
            service.CreateUser(new CreateUserRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@email.com",
                Password = "password123",
                City = "Rome",        // Posso cambiare ordine
                Country = "Italy"     // senza problemi!
            });
        }
    }

    // ✅ SOLUZIONE 3: Builder Pattern (per oggetti complessi)
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }

    public class UserBuilder
    {
        private readonly User _user = new User();

        public UserBuilder WithName(string firstName, string lastName)
        {
            _user.FirstName = firstName;
            _user.LastName = lastName;
            return this;
        }

        public UserBuilder WithEmail(string email)
        {
            _user.Email = email;
            return this;
        }

        public UserBuilder WithPassword(string password)
        {
            _user.Password = password;
            return this;
        }

        public UserBuilder WithLocation(string country, string city)
        {
            _user.Country = country;
            _user.City = city;
            return this;
        }

        public User Build() => _user;
    }

    public class Example3
    {
        public void Test()
        {
            // Fluent interface - leggibile e flessibile!
            var user = new UserBuilder()
                .WithName("John", "Doe")
                .WithEmail("john@email.com")
                .WithPassword("password123")
                .WithLocation("Italy", "Rome")
                .Build();
        }
    }

    // VANTAGGI:
    // - Named parameters: ordine non importa
    // - Parameter Object: facile aggiungere nuovi parametri
    // - Builder: costruzione fluente e leggibile
}
