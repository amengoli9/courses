// OCP — VIOLAZIONE: aggiungere "Enterprise" richiede di modificare Apply().
// Si tocca codice già testato e si rischia di rompere i casi esistenti.

public class DiscountCalculator
{
    public decimal Apply(Order order)
    {
        if (order.Customer.Type == "Standard")  return order.Total * 0.95m;
        if (order.Customer.Type == "Premium")   return order.Total * 0.90m;
        if (order.Customer.Type == "Vip")       return order.Total * 0.85m;
        return order.Total;
    }
}

// Ogni nuovo tipo di cliente = un nuovo if = una modifica al codice esistente.
// Con 10 tipi, questo metodo cresce in modo lineare e indefinito.
