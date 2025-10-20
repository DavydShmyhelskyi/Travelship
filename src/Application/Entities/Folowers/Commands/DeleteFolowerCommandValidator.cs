using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Folowers.Commands
{
    public class DeleteFollowerCommandValidator : AbstractValidator<DeleteFoll>
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
}
