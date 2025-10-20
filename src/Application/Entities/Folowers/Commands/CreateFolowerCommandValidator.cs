using Application.Entities.Followers.Commands;
using FluentValidation;

namespace Application.Entities.Folowers.Commands;

public class CreateFollowerCommandValidator : AbstractValidator<CreateFollowerCommand>
{
    public CreateFollowerCommandValidator()
    {
        RuleFor(x => x.FollowerUserId)
            .NotEmpty().WithMessage("FollowerId is required.");

        RuleFor(x => x.FollowedUserId)
            .NotEmpty().WithMessage("FollowedId is required.")
            .NotEqual(x => x.FollowerUserId)
            .WithMessage("FollowerId and FollowedId cannot be the same.");
    }
}
