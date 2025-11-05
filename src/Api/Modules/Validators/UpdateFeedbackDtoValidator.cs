using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators
{
    public class UpdateFeedbackDtoValidator : AbstractValidator<UpdateFeedbackDto>
    {
        public UpdateFeedbackDtoValidator()
        {
            RuleFor(x => x.Comment)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(500);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5);
        }
    }
}