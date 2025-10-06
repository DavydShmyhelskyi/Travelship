using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateCityDtoValidator : AbstractValidator<CreateCityDto>
{
    public CreateCityDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(2);

        RuleFor(x => x.CountryId)
            .NotEmpty();
    }
}
