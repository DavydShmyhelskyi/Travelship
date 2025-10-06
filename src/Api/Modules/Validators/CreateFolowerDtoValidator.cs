using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateFollowerDtoValidator : AbstractValidator<CreateFollowerDto>
{
    public CreateFollowerDtoValidator()
    {
        RuleFor(x => x.FollowerId)
            .NotEmpty();

        RuleFor(x => x.FollowedId)
            .NotEmpty()
            .NotEqual(x => x.FollowerId).WithMessage("Follower and Followed cannot be the same user");
    }
}
