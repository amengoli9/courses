// OCP — CORRETTO: Strategy pattern.
// Aggiungere "Enterprise" = nuova classe + entry nella mappa. Zero modifiche all'esistente.

public interface IDiscountStrategy
{
    decimal Apply(decimal total);
}

public class StandardDiscount : IDiscountStrategy
{
    public decimal Apply(decimal total) => total * 0.95m;
}

public class PremiumDiscount : IDiscountStrategy
{
    public decimal Apply(decimal total) => total * 0.90m;
}

public class VipDiscount : IDiscountStrategy
{
    public decimal Apply(decimal total) => total * 0.85m;
}

public class NoDiscount : IDiscountStrategy
{
    public decimal Apply(decimal total) => total;
}

// Per aggiungere Enterprise: crea EnterpriseDiscount e aggiungila alla mappa. Fine.
// public class EnterpriseDiscount : IDiscountStrategy
// {
//     public decimal Apply(decimal total) => total * 0.80m;
// }

public class DiscountCalculator
{
    private readonly IDictionary<CustomerType, IDiscountStrategy> _strategies;
    private readonly IDiscountStrategy _default;

    public DiscountCalculator(
        IDictionary<CustomerType, IDiscountStrategy> strategies,
        IDiscountStrategy defaultStrategy)
    {
        _strategies = strategies;
        _default = defaultStrategy;
    }

    public decimal Apply(Order order)
    {
        var strategy = _strategies.TryGetValue(order.Customer.Type, out var s)
            ? s
            : _default;
        return strategy.Apply(order.Total);
    }
}
