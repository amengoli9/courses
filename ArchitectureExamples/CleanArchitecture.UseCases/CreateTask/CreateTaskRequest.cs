namespace CleanArchitecture.UseCases.CreateTask;

/// <summary>
/// REQUEST - Dati in ingresso per il caso d'uso
/// </summary>
public record CreateTaskRequest(string Title, string Description);
