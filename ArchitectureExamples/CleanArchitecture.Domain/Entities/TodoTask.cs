namespace CleanArchitecture.Domain.Entities;

/// <summary>
/// ENTITÀ - Layer più interno della Clean Architecture
/// Contiene la logica di business fondamentale
/// Non dipende da nulla, nemmeno dagli Use Cases
/// </summary>
public class TodoTask
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public TodoTask(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        Id = Guid.NewGuid();
        Title = title;
        Description = description ?? string.Empty;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new InvalidOperationException("Task is already completed");

        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }

    public void Reopen()
    {
        if (!IsCompleted)
            throw new InvalidOperationException("Task is not completed");

        IsCompleted = false;
        CompletedAt = null;
    }

    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Title cannot be empty", nameof(newTitle));

        Title = newTitle;
    }
}
