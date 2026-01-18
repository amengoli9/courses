namespace CleanArchitecture.UseCases.CreateTask;

/// <summary>
/// RESPONSE - Dati in uscita del caso d'uso
/// </summary>
public record CreateTaskResponse(Guid Id, string Title, DateTime CreatedAt);
