using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Feedbacks.Commands
{
    public class UpdateFeedbackCommandValidator : AbstractValidator<UpdateFeedbackCommand>
    {
        public UpdateFeedbackCommandValidator()
        {
            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required.")
                .MinimumLength(2)
                .MaximumLength(1000).WithMessage("Comment must be less than 1000 characters.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
        }
    }
}
