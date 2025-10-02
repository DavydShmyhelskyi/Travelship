using FluentValidation;

namespace Application.Entities.Cities.Commands;

public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(2).WithMessage("Title must be at least 2 characters.")
            .MaximumLength(100).WithMessage("Title must be less than 100 characters.");

        RuleFor(x => x.CountryId)
            .NotEmpty().WithMessage("CountryId is required.");
    }
}
