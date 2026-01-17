/*
 * ============================================================================
 * 3. CONNASCENCE OF MEANING (CoM)
 * ============================================================================
 *
 * DEFINIZIONE: Più componenti devono concordare sul SIGNIFICATO di valori
 * FORZA: ⭐⭐⭐ MEDIA-ALTA (pericolosa!)
 *
 * ============================================================================
 */

// ============================================================================
// ESEMPIO SEMPLICE PER SLIDE
// ============================================================================

/*
PROBLEMA:
    if (status == 1) { }  // 1 = cosa? Pending? Active?
    if (status == 2) { }  // 2 = cosa?

SOLUZIONE:
    if (status == OrderStatus.Pending) { }  // Chiaro ed esplicito!
    if (status == OrderStatus.Shipped) { }
*/

// ============================================================================
// ESEMPIO C#
// ============================================================================

namespace ConnascenceCourse.ConnascenceOfMeaning
{
    // ❌ PROBLEMA: Magic numbers e magic strings
    public class OrderService
    {
        public void ProcessOrder(int orderId, int status)
        {
            // Cosa significa 1? 2? 3?
            if (status == 1)
            {
                Console.WriteLine("Order pending");
            }
            else if (status == 2)
            {
                Console.WriteLine("Order shipped");
            }
            else if (status == 3)
            {
                Console.WriteLine("Order delivered");
            }
        }

        public decimal CalculatePrice(decimal basePrice, bool includeVat)
        {
            if (includeVat)
            {
                return basePrice * 1.22m; // Magic number! 1.22 = cosa? IVA italiana?
            }
            return basePrice;
        }
    }

    public class UserService
    {
        public bool HasPermission(string role, string action)
        {
            // Magic strings - facile fare typo!
            if (role == "admin") return true;
            if (role == "user" && action == "read") return true;
            return false;
        }
    }

    // PROBLEMA: Devo ricordare cosa significano 1, 2, 3, "admin", "user", 1.22

    // ✅ SOLUZIONE: Enum e costanti con nomi espliciti
    public enum OrderStatus
    {
        Pending = 1,
        Shipped = 2,
        Delivered = 3
    }

    public enum UserRole
    {
        Admin,
        User,
        Guest
    }

    public static class TaxRates
    {
        public const decimal ItalianVAT = 0.22m; // 22%
    }

    public class OrderServiceRefactored
    {
        public void ProcessOrder(int orderId, OrderStatus status)
        {
            // Chiaro ed esplicito!
            switch (status)
            {
                case OrderStatus.Pending:
                    Console.WriteLine("Order pending");
                    break;
                case OrderStatus.Shipped:
                    Console.WriteLine("Order shipped");
                    break;
                case OrderStatus.Delivered:
                    Console.WriteLine("Order delivered");
                    break;
            }
        }

        public decimal CalculatePrice(decimal basePrice, bool includeVat)
        {
            if (includeVat)
            {
                return basePrice * (1 + TaxRates.ItalianVAT); // Chiaro!
            }
            return basePrice;
        }
    }

    public class UserServiceRefactored
    {
        public bool HasPermission(UserRole role, string action)
        {
            // Type-safe, no typos possible!
            return role switch
            {
                UserRole.Admin => true,
                UserRole.User => action == "read",
                UserRole.Guest => false,
                _ => false
            };
        }
    }

    // VANTAGGI:
    // - Codice auto-documentante
    // - Impossibile usare valori non validi
    // - IntelliSense mostra tutti i valori possibili
    // - No typos: "admni" vs "admin"
}
