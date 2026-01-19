using HexagonalArchitecture.Domain;
using HexagonalArchitecture.Domain.Ports;
using HexagonalArchitecture.Infrastructure.Adapters;

namespace HexagonalArchitecture.Api;

/// <summary>
/// Entry point dell'applicazione
/// Qui viene fatto il WIRING: collegamento tra le PORTE e gli ADAPTER
/// Nell'architettura esagonale, l'API (driver adapter) usa direttamente le porte del dominio
/// Questa è l'unica parte che conosce sia il dominio che l'infrastruttura
/// </summary>
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== HEXAGONAL ARCHITECTURE EXAMPLE ===\n");

        // Dependency Injection manuale (in una app reale useresti un DI Container)
        IOrderRepository orderRepository = new InMemoryOrderRepository();
        INotificationService notificationService = new ConsoleNotificationService();

        // Caso d'uso: Creare un ordine
        Console.WriteLine("1. Creating new order...");
        var order = new Order("Mario Rossi", 150.50m);
        await orderRepository.SaveAsync(order);
        Console.WriteLine($"   Order created: {order.Id}\n");

        // Caso d'uso: Ottenere un ordine
        Console.WriteLine("2. Retrieving order...");
        var retrievedOrder = await orderRepository.GetByIdAsync(order.Id);
        Console.WriteLine($"   Order found: {retrievedOrder?.CustomerName}, Status: {retrievedOrder?.Status}\n");

        // Caso d'uso: Confermare un ordine
        Console.WriteLine("3. Confirming order...");
        var orderToConfirm = await orderRepository.GetByIdAsync(order.Id);
        if (orderToConfirm != null)
        {
            orderToConfirm.Confirm();
            await orderRepository.SaveAsync(orderToConfirm);
            await notificationService.NotifyOrderConfirmedAsync(orderToConfirm);
        }
        Console.WriteLine($"   Order status: {orderToConfirm?.Status}\n");

        // Caso d'uso: Ottenere tutti gli ordini
        Console.WriteLine("4. Getting all orders...");
        var allOrders = await orderRepository.GetAllAsync();
        Console.WriteLine($"   Total orders: {allOrders.Count()}\n");

        Console.WriteLine("=== FINE ===");
    }
}
