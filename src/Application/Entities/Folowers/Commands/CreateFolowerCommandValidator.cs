using FluentValidation;

namespace Application.Entities.Folowers.Commands;

public class CreateFollowerCommandValidator : AbstractValidator<CreateFollowerCommand>
{
    public CreateFollowerCommandValidator()
    {
        RuleFor(x => x.FollowerId)
            .NotEmpty().WithMessage("FollowerId is required.");

        RuleFor(x => x.FollowedId)
            .NotEmpty().WithMessage("FollowedId is required.")
            .NotEqual(x => x.FollowerId)
            .WithMessage("FollowerId and FollowedId cannot be the same.");
    }
}
