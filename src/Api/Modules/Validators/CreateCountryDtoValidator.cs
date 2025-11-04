using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateCountryDtoValidator : AbstractValidator<CreateCountryDto>
{
    public CreateCountryDtoValidator()
    {
        RuleFor(x => x.Title)
    .NotEmpty()
    .MinimumLength(3)
    .MaximumLength(100);
    }
}
