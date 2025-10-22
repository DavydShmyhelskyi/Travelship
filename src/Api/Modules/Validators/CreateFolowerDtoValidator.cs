using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateFollowerDtoValidator : AbstractValidator<CreateFollowerDto>
{
    public CreateFollowerDtoValidator()
    {
        RuleFor(x => x.FollowerUserId)
            .NotEmpty();

        RuleFor(x => x.FollowedUserId)
            .NotEmpty()
            .NotEqual(x => x.FollowerUserId)
            .WithMessage("User cannot follow themselves.");
    }
}
