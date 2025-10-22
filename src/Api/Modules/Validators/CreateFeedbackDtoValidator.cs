using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateFeedbackDtoValidator : AbstractValidator<CreateFeedbackDto>
{
    public CreateFeedbackDtoValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(500);

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5);

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.PlaceId)
            .NotEmpty();
    }
}
