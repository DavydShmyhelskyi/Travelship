using FluentValidation;

namespace Application.Entities.Users.Commands
{
    public class ChangeUserPasswordCommandValidator : FluentValidation.AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangeUserPasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}
