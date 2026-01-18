using HexagonalArchitecture.Domain;
using HexagonalArchitecture.Domain.Ports;

namespace HexagonalArchitecture.Infrastructure.Adapters;

/// <summary>
/// ADAPTER - Implementazione concreta della porta IOrderRepository
/// Questo adapter usa la memoria (in-memory) per persistere gli ordini
/// Potrebbe essere facilmente sostituito con un adapter per SQL, MongoDB, ecc.
/// </summary>
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> _orders = new();

    public Task<Order?> GetByIdAsync(Guid id)
    {
        _orders.TryGetValue(id, out var order);
        return Task.FromResult(order);
    }

    public Task<IEnumerable<Order>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Order>>(_orders.Values.ToList());
    }

    public Task SaveAsync(Order order)
    {
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _orders.Remove(id);
        return Task.CompletedTask;
    }
}
