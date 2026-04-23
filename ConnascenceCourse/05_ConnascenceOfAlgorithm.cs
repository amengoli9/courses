/*
 * ============================================================================
 * 5. CONNASCENCE OF ALGORITHM (CoA)
 * ============================================================================
 *
 * DEFINIZIONE: Più componenti devono concordare su uno specifico ALGORITMO
 * FORZA: ⭐⭐⭐⭐ ALTA (pericolosa!)
 *
 * ============================================================================
 */

// ============================================================================
// ESEMPIO SEMPLICE PER SLIDE
// ============================================================================

/*
PROBLEMA:
    // Client
    string hash = MD5(password + "salt123")

    // Server
    string hash = MD5(password + "salt123")  // Stesso algoritmo duplicato!

SOLUZIONE:
    // Servizio condiviso
    IHashingService.Hash(password)  // Algoritmo centralizzato
*/

// ============================================================================
// ESEMPIO C#
// ============================================================================

namespace ConnascenceCourse.ConnascenceOfAlgorithm
{
    using System.Security.Cryptography;
    using System.Text;

    // ❌ PROBLEMA: Algoritmo duplicato in più posti
    public class LoginService
    {
        public bool ValidatePassword(string inputPassword, string storedHash)
        {
            // Algoritmo di hashing duplicato
            string hash = ComputeHash(inputPassword);
            return hash == storedHash;
        }

        private string ComputeHash(string input)
        {
            using var md5 = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(input + "salt123");
            byte[] hash = md5.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    public class RegistrationService
    {
        public string CreateUser(string username, string password)
        {
            // STESSO algoritmo ma DUPLICATO!
            string hash = ComputeHash(password);
            Console.WriteLine($"User {username} created with hash {hash}");
            return hash;
        }

        private string ComputeHash(string input)
        {
            // Se cambio l'algoritmo qui, devo cambiarlo anche in LoginService!
            using var md5 = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(input + "salt123");
            byte[] hash = md5.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    // Se i due algoritmi divergono, gli utenti non possono più fare login!

    // ✅ SOLUZIONE: Centralizza l'algoritmo
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }

    public class PasswordHasher : IPasswordHasher
    {
        private const string Salt = "salt123";

        public string HashPassword(string password)
        {
            using var md5 = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password + Salt);
            byte[] hash = md5.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(string password, string hash)
        {
            string computedHash = HashPassword(password);
            return computedHash == hash;
        }
    }

    public class LoginServiceRefactored
    {
        private readonly IPasswordHasher _hasher;

        public LoginServiceRefactored(IPasswordHasher hasher)
        {
            _hasher = hasher;
        }

        public bool ValidatePassword(string inputPassword, string storedHash)
        {
            return _hasher.VerifyPassword(inputPassword, storedHash);
        }
    }

    public class RegistrationServiceRefactored
    {
        private readonly IPasswordHasher _hasher;

        public RegistrationServiceRefactored(IPasswordHasher hasher)
        {
            _hasher = hasher;
        }

        public string CreateUser(string username, string password)
        {
            string hash = _hasher.HashPassword(password);
            Console.WriteLine($"User {username} created");
            return hash;
        }
    }

    // VANTAGGI:
    // - Algoritmo in UN solo posto
    // - Se cambio algoritmo, cambio una sola classe
    // - Consistenza garantita
    // - Facile da testare
}
