using FluentValidation;

namespace Application.Entities.Roles.Commands
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters.").MaximumLength(255).WithMessage("Title must be less then 255 characters");
        }
    }
}
