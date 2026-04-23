/*
 * ============================================================================
 * 8. CONNASCENCE OF VALUE (CoV)
 * ============================================================================
 *
 * DEFINIZIONE: Più componenti devono concordare su VALORI correlati
 * FORZA: ⭐⭐⭐⭐⭐ MOLTO ALTA (estremamente pericolosa!)
 *
 * ============================================================================
 */

// ============================================================================
// ESEMPIO SEMPLICE PER SLIDE
// ============================================================================

/*
PROBLEMA:
    class Order {
        decimal totalPrice;
        decimal totalWithTax;  // DEVE essere = totalPrice * 1.22
    }
    // I due valori devono essere sincronizzati!

SOLUZIONE:
    class Order {
        decimal totalPrice;
        decimal TotalWithTax => totalPrice * 1.22;  // Calcolato automaticamente
    }
*/

// ============================================================================
// ESEMPIO C#
// ============================================================================

namespace ConnascenceCourse.ConnascenceOfValue
{
    // ❌ PROBLEMA: Valori duplicati che devono essere sincronizzati
    public class ShoppingCart
    {
        private List<decimal> _itemPrices = new();
        private decimal _subtotal;
        private decimal _tax;
        private decimal _total;

        public void AddItem(decimal price)
        {
            _itemPrices.Add(price);

            // Devo aggiornare TUTTI i valori correlati!
            _subtotal = _itemPrices.Sum();
            _tax = _subtotal * 0.22m;
            _total = _subtotal + _tax;
        }

        public void RemoveItem(decimal price)
        {
            _itemPrices.Remove(price);

            // Devo ricordarmi di aggiornare di nuovo!
            _subtotal = _itemPrices.Sum();
            _tax = _subtotal * 0.22m;
            _total = _subtotal + _tax;
        }

        // Cosa succede se dimentico di aggiornare un valore?
        public void ApplyDiscount(decimal discount)
        {
            _subtotal -= discount;
            // ❌ OOPS! Ho dimenticato di aggiornare tax e total!
        }

        public decimal GetTotal() => _total;
    }

    // ✅ SOLUZIONE 1: Computed Properties (valori derivati)
    public class ShoppingCartRefactored1
    {
        private List<decimal> _itemPrices = new();

        public void AddItem(decimal price)
        {
            _itemPrices.Add(price);
            // Non serve aggiornare altro!
        }

        public void RemoveItem(decimal price)
        {
            _itemPrices.Remove(price);
            // Non serve aggiornare altro!
        }

        // Valori calcolati on-the-fly
        public decimal Subtotal => _itemPrices.Sum();
        public decimal Tax => Subtotal * 0.22m;
        public decimal Total => Subtotal + Tax;

        // Impossibile dimenticare di aggiornare - sono sempre consistenti!
    }

    // ✅ SOLUZIONE 2: Value Object immutabile
    public record CartTotals(decimal Subtotal, decimal Tax, decimal Total)
    {
        // Factory method che garantisce consistenza
        public static CartTotals Calculate(List<decimal> itemPrices)
        {
            var subtotal = itemPrices.Sum();
            var tax = subtotal * 0.22m;
            var total = subtotal + tax;
            return new CartTotals(subtotal, tax, total);
        }
    }

    public class ShoppingCartRefactored2
    {
        private List<decimal> _itemPrices = new();

        public void AddItem(decimal price)
        {
            _itemPrices.Add(price);
        }

        public void RemoveItem(decimal price)
        {
            _itemPrices.Remove(price);
        }

        // Sempre consistente
        public CartTotals GetTotals() => CartTotals.Calculate(_itemPrices);
    }

    // ✅ SOLUZIONE 3: Evita ridondanza - single source of truth
    public class Rectangle
    {
        // ❌ BAD: width, height, area tutti memorizzati
        // public decimal Width { get; set; }
        // public decimal Height { get; set; }
        // public decimal Area { get; set; } // Ridondante!

        // ✅ GOOD: solo width e height, area calcolata
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Area => Width * Height; // Sempre corretto!
    }

    // Esempio più complesso: Report con dati aggregati
    public class SalesReport
    {
        private List<decimal> _sales = new();

        // ❌ BAD: memorizzare aggregati
        // private decimal _totalSales;
        // private decimal _averageSale;
        // private int _numberOfSales;

        // ✅ GOOD: calcolare on-demand
        public decimal TotalSales => _sales.Sum();
        public decimal AverageSale => _sales.Any() ? _sales.Average() : 0;
        public int NumberOfSales => _sales.Count;

        public void AddSale(decimal amount)
        {
            _sales.Add(amount);
            // Nessun altro update necessario!
        }
    }

    // Pattern: Cache con invalidazione
    public class ShoppingCartWithCache
    {
        private List<decimal> _itemPrices = new();
        private decimal? _cachedTotal; // Cache opzionale per performance

        public void AddItem(decimal price)
        {
            _itemPrices.Add(price);
            _cachedTotal = null; // Invalida cache
        }

        public decimal Total
        {
            get
            {
                if (_cachedTotal == null)
                {
                    // Ricalcola solo se necessario
                    var subtotal = _itemPrices.Sum();
                    var tax = subtotal * 0.22m;
                    _cachedTotal = subtotal + tax;
                }
                return _cachedTotal.Value;
            }
        }
    }

    // VANTAGGI:
    // - Computed properties: sempre sincronizzate
    // - Value Objects: consistenza garantita
    // - Single source of truth: nessuna duplicazione
    // - Cache: performance mantenendo consistenza
}
