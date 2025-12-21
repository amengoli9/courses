using SantasWorkshop.Interfaces;
using SantasWorkshop.Models;

namespace SantasWorkshop.Services;

/// <summary>
/// Implementazione del validatore di comportamento
/// </summary>
public class BehaviorValidator : IBehaviorValidator
{
    public BehaviorValidationResult Validate(string behavior, int age)
    {
        if (behavior == "Cattivo")
        {
            return new BehaviorValidationResult
            {
                IsValid = false,
                Message = "è stato cattivo! Riceverà carbone!"
            };
        }
        else if (behavior == "Birichino")
        {
            if (age < 6)
            {
                return new BehaviorValidationResult
                {
                    IsValid = true,
                    Message = "è stato birichino... valutazione in corso\nÈ piccolo, gli diamo una possibilità!"
                };
            }
            else
            {
                return new BehaviorValidationResult
                {
                    IsValid = false,
                    Message = "è stato birichino... valutazione in corso\nTroppo grande per fare il birichino! Carbone!"
                };
            }
        }

        return new BehaviorValidationResult
        {
            IsValid = true,
            Message = string.Empty
        };
    }
}
