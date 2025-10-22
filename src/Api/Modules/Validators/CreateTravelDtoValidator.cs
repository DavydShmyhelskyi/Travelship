using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateTravelDtoValidator : AbstractValidator<CreateTravelDto>
{
    public CreateTravelDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(1000);

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("StartDate must be before EndDate.");

        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
