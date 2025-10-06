using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreatePlaceDtoValidator : AbstractValidator<CreatePlaceDto>
{
    public CreatePlaceDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180);
    }
}
