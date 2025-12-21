using SantasWorkshop.Models;

namespace SantasWorkshop.Interfaces;

/// <summary>
/// [S] Processa le lettere di Natale
/// </summary>
public interface ILetterProcessor
{
    void ProcessLetter(LetterRequest request);
}
