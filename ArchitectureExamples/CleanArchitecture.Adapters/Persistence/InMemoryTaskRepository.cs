using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Adapters.Persistence;

/// <summary>
/// ADAPTER - Implementazione del repository
/// Questo layer sta all'esterno e implementa le interfacce definite nel Domain
/// Potrebbe essere facilmente sostituito con EF Core, Dapper, ecc.
/// </summary>
public class InMemoryTaskRepository : ITaskRepository
{
    private readonly Dictionary<Guid, TodoTask> _tasks = new();

    public Task<TodoTask?> GetByIdAsync(Guid id)
    {
        _tasks.TryGetValue(id, out var task);
        return Task.FromResult(task);
    }

    public Task<IEnumerable<TodoTask>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<TodoTask>>(_tasks.Values.ToList());
    }

    public Task<IEnumerable<TodoTask>> GetCompletedAsync()
    {
        var completed = _tasks.Values.Where(t => t.IsCompleted).ToList();
        return Task.FromResult<IEnumerable<TodoTask>>(completed);
    }

    public Task<IEnumerable<TodoTask>> GetPendingAsync()
    {
        var pending = _tasks.Values.Where(t => !t.IsCompleted).ToList();
        return Task.FromResult<IEnumerable<TodoTask>>(pending);
    }

    public Task AddAsync(TodoTask task)
    {
        _tasks[task.Id] = task;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TodoTask task)
    {
        _tasks[task.Id] = task;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _tasks.Remove(id);
        return Task.CompletedTask;
    }
}
