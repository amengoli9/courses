using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.UseCases.CreateTask;

/// <summary>
/// USE CASE - Rappresenta un caso d'uso specifico dell'applicazione
/// Contiene la logica applicativa (non la logica di business che sta nelle entità)
/// Dipende solo dal Domain, non conosce i dettagli di implementazione
/// </summary>
public class CreateTaskUseCase
{
    private readonly ITaskRepository _taskRepository;

    public CreateTaskUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    }

    public async Task<CreateTaskResponse> ExecuteAsync(CreateTaskRequest request)
    {
        // Validazione input
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        // Creazione entità (la logica di business è nell'entità)
        var task = new TodoTask(request.Title, request.Description);

        // Persistenza tramite repository
        await _taskRepository.AddAsync(task);

        // Ritorno response
        return new CreateTaskResponse(task.Id, task.Title, task.CreatedAt);
    }
}
