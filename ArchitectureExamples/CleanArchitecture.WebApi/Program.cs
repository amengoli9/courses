using CleanArchitecture.Adapters.Persistence;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.UseCases.CompleteTask;
using CleanArchitecture.UseCases.CreateTask;
using CleanArchitecture.UseCases.GetAllTasks;

namespace CleanArchitecture.WebApi;

/// <summary>
/// Entry point - Layer più esterno
/// Gestisce il wiring delle dipendenze e l'esecuzione dell'applicazione
/// Questo layer conosce tutti gli altri layer
/// </summary>
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== CLEAN ARCHITECTURE EXAMPLE ===\n");

        // Dependency Injection (manuale per semplicità)
        ITaskRepository taskRepository = new InMemoryTaskRepository();

        // Creazione Use Cases
        var createTaskUseCase = new CreateTaskUseCase(taskRepository);
        var getAllTasksUseCase = new GetAllTasksUseCase(taskRepository);
        var completeTaskUseCase = new CompleteTaskUseCase(taskRepository);

        // Scenario: Creare task
        Console.WriteLine("1. Creating tasks...");
        var task1 = await createTaskUseCase.ExecuteAsync(new CreateTaskRequest("Studiare Clean Architecture", "Leggere libro di Uncle Bob"));
        Console.WriteLine($"   Task created: {task1.Title} (ID: {task1.Id})");

        var task2 = await createTaskUseCase.ExecuteAsync(new CreateTaskRequest("Fare la spesa", "Comprare latte e pane"));
        Console.WriteLine($"   Task created: {task2.Title} (ID: {task2.Id})\n");

        // Scenario: Ottenere tutti i task
        Console.WriteLine("2. Getting all tasks...");
        var allTasks = await getAllTasksUseCase.ExecuteAsync();
        foreach (var task in allTasks.Tasks)
        {
            Console.WriteLine($"   - {task.Title} [{(task.IsCompleted ? "✓" : " ")}]");
        }
        Console.WriteLine();

        // Scenario: Completare un task
        Console.WriteLine("3. Completing first task...");
        await completeTaskUseCase.ExecuteAsync(task1.Id);
        Console.WriteLine($"   Task {task1.Title} completed!\n");

        // Scenario: Vedere lo stato aggiornato
        Console.WriteLine("4. Updated task list...");
        allTasks = await getAllTasksUseCase.ExecuteAsync();
        foreach (var task in allTasks.Tasks)
        {
            Console.WriteLine($"   - {task.Title} [{(task.IsCompleted ? "✓" : " ")}]");
        }

        Console.WriteLine("\n=== FINE ===");
    }
}
