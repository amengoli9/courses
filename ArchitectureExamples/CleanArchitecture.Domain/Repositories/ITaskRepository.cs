using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Repositories;

/// <summary>
/// INTERFACCIA REPOSITORY - Definita nel Domain
/// Nella Clean Architecture, il Domain definisce le interfacce
/// Gli adapter (layer esterni) le implementano
/// </summary>
public interface ITaskRepository
{
    Task<TodoTask?> GetByIdAsync(Guid id);
    Task<IEnumerable<TodoTask>> GetAllAsync();
    Task<IEnumerable<TodoTask>> GetCompletedAsync();
    Task<IEnumerable<TodoTask>> GetPendingAsync();
    Task AddAsync(TodoTask task);
    Task UpdateAsync(TodoTask task);
    Task DeleteAsync(Guid id);
}
