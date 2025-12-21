namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Assegna elfi ai paesi
/// [O] Estendibile tramite nuove implementazioni
/// </summary>
public interface IElfAssignmentService
{
    string AssignElf(string country);
}
