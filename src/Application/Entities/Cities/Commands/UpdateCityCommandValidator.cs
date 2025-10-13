using FluentValidation;

namespace Application.Entities.Cities.Commands;

public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
{
    public UpdateCityCommandValidator()
    {
        RuleFor(x => x.CityId).NotEmpty();
        RuleFor(x => x.CountryId).NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters.")
            .MaximumLength(255).WithMessage("Title must be less than 255 characters.");
    }
}
