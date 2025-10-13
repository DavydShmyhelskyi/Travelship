using FluentValidation;

namespace Application.Entities.Users.Commands
{
    public class UpdateUserCommandValidator : FluentValidation.AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.NickName)
            .NotEmpty().WithMessage("NickName is required.")
            .MinimumLength(3).WithMessage("NickName must be at least 3 characters.")
            .MaximumLength(50).WithMessage("NickName must be less than 50 characters.");

            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("RoleId is required.");
        }
    }
}
