using FluentValidation;

namespace Application.Entities.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.NickName)
            .NotEmpty().WithMessage("NickName is required.")
            .MinimumLength(3).WithMessage("NickName must be at least 3 characters.")
            .MaximumLength(50).WithMessage("NickName must be less than 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("RoleId is required.");

    }
}
