using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateTravelDtoValidator : AbstractValidator<CreateTravelDto>
{
    public CreateTravelDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate).WithMessage("StartDate must be before EndDate");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
