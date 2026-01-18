using HexagonalArchitecture.Application;
using HexagonalArchitecture.Domain.Ports;
using HexagonalArchitecture.Infrastructure.Adapters;

namespace HexagonalArchitecture.Api;

/// <summary>
/// Entry point dell'applicazione
/// Qui viene fatto il WIRING: collegamento tra le PORTE e gli ADAPTER
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
        var orderService = new OrderService(orderRepository, notificationService);

        // Caso d'uso: Creare un ordine
        Console.WriteLine("1. Creating new order...");
        var order = await orderService.CreateOrderAsync("Mario Rossi", 150.50m);
        Console.WriteLine($"   Order created: {order.Id}\n");

        // Caso d'uso: Ottenere un ordine
        Console.WriteLine("2. Retrieving order...");
        var retrievedOrder = await orderService.GetOrderAsync(order.Id);
        Console.WriteLine($"   Order found: {retrievedOrder?.CustomerName}, Status: {retrievedOrder?.Status}\n");

        // Caso d'uso: Confermare un ordine
        Console.WriteLine("3. Confirming order...");
        await orderService.ConfirmOrderAsync(order.Id);
        Console.WriteLine($"   Order status: {retrievedOrder?.Status}\n");

        // Caso d'uso: Ottenere tutti gli ordini
        Console.WriteLine("4. Getting all orders...");
        var allOrders = await orderService.GetAllOrdersAsync();
        Console.WriteLine($"   Total orders: {allOrders.Count()}\n");

        Console.WriteLine("=== FINE ===");
    }
}
