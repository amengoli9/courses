using FluentValidation;
using Kitchen.WebApi.DTOs;

namespace Kitchen.WebApi.Validators;

public class UpdateMenuItemRequestValidator : AbstractValidator<UpdateMenuItemRequest>
{
    public UpdateMenuItemRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Il nome è obbligatorio")
            .MaximumLength(100).WithMessage("Il nome non può superare 100 caratteri");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descrizione è obbligatoria")
            .MaximumLength(500).WithMessage("La descrizione non può superare 500 caratteri");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Il prezzo deve essere maggiore di zero");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("La categoria non è valida");

        RuleFor(x => x.PreparationTimeMinutes)
            .GreaterThanOrEqualTo(0).WithMessage("Il tempo di preparazione non può essere negativo");
    }
}
