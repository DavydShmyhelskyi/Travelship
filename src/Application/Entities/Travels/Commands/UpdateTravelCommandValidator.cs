using FluentValidation;

namespace Application.Entities.Travels.Commands
{
    public class UpdateTravelCommandValidator : AbstractValidator<UpdateTravelCommand>
    {
        public UpdateTravelCommandValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate)
                .WithMessage("Start date must be before or equal to end date.");

            RuleFor(x => x.Places)
                .NotEmpty()
                .WithMessage("Travel must contain at least one place.");

            RuleFor(x => x.Members)
                .NotEmpty()
                .WithMessage("Travel must have at least one member.");
        }
    }
}
