using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.UseCases.CompleteTask;

/// <summary>
/// USE CASE - Completare un task
/// </summary>
public class CompleteTaskUseCase
{
    private readonly ITaskRepository _taskRepository;

    public CompleteTaskUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    }

    public async Task ExecuteAsync(Guid taskId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
            throw new InvalidOperationException($"Task {taskId} not found");

        task.Complete();
        await _taskRepository.UpdateAsync(task);
    }
}
