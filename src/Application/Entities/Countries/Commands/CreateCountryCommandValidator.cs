using FluentValidation;

namespace Application.Entities.Countries.Commands
{
    public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters.")
                .MaximumLength(255).WithMessage("Title must be less then 255 characters");
        }
    }
}
