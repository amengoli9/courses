// DRY — ATTENZIONE: duplicazione APPARENTE, non vera.
// Le due funzioni sembrano identiche ma rispondono a regole di business diverse:
// spedizione e stoccaggio cambieranno indipendentemente.
// Unificarle sarebbe una "falsa DRY".

public class OrderService
{
    public decimal CalculateShippingCost(Order order)
    {
        var weight = order.TotalWeight;
        return weight * 2.5m;  // €2.50 al kg (tariffa spedizioniere)
    }
}

public class WarehouseService
{
    public decimal CalculateStorageFee(Inventory inventory)
    {
        var weight = inventory.TotalWeight;
        return weight * 2.5m;  // €2.50 al kg di stoccaggio mensile (tariffa magazzino)
    }
}

// Regola pratica: prima di unire, chiediti:
// "Se questa formula cambiasse, le due chiamate cambierebbero INSIEME?"
// Se la risposta è NO → non unire.
