namespace HexagonalArchitecture.Domain.Ports;

/// <summary>
/// PORTA (Port) - Interfaccia definita dal dominio
/// Specifica COSA serve al dominio senza specificare COME viene implementato
/// Gli adapter implementeranno questa interfaccia
/// </summary>
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task SaveAsync(Order order);
    Task DeleteAsync(Guid id);
}
