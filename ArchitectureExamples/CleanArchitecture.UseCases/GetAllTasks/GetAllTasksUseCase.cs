using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.UseCases.GetAllTasks;

/// <summary>
/// USE CASE - Ottenere tutti i task
/// </summary>
public class GetAllTasksUseCase
{
    private readonly ITaskRepository _taskRepository;

    public GetAllTasksUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    }

    public async Task<GetAllTasksResponse> ExecuteAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();

        var taskDtos = tasks.Select(t => new TaskDto(
            t.Id,
            t.Title,
            t.Description,
            t.IsCompleted,
            t.CreatedAt,
            t.CompletedAt
        )).ToList();

        return new GetAllTasksResponse(taskDtos);
    }
}

public record TaskDto(Guid Id, string Title, string Description, bool IsCompleted, DateTime CreatedAt, DateTime? CompletedAt);
public record GetAllTasksResponse(IEnumerable<TaskDto> Tasks);
