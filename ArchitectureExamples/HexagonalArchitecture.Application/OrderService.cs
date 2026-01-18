using HexagonalArchitecture.Domain;
using HexagonalArchitecture.Domain.Ports;

namespace HexagonalArchitecture.Application;

/// <summary>
/// Servizio Applicativo - Coordina i casi d'uso
/// Usa le PORTE (Ports) definite dal dominio
/// Non dipende dagli ADAPTER concreti, solo dalle interfacce
/// </summary>
public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly INotificationService _notificationService;

    public OrderService(IOrderRepository orderRepository, INotificationService notificationService)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }

    public async Task<Order> CreateOrderAsync(string customerName, decimal totalAmount)
    {
        var order = new Order(customerName, totalAmount);
        await _orderRepository.SaveAsync(order);
        return order;
    }

    public async Task<Order?> GetOrderAsync(Guid id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task ConfirmOrderAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            throw new InvalidOperationException($"Order {id} not found");

        order.Confirm();
        await _orderRepository.SaveAsync(order);
        await _notificationService.NotifyOrderConfirmedAsync(order);
    }

    public async Task CancelOrderAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            throw new InvalidOperationException($"Order {id} not found");

        order.Cancel();
        await _orderRepository.SaveAsync(order);
        await _notificationService.NotifyOrderCancelledAsync(order);
    }
}
