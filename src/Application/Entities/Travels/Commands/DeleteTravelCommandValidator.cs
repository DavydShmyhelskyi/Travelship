using FluentValidation;

namespace Application.Entities.Travels.Commands
{
    public class DeleteTravelCommandValidator : AbstractValidator<DeleteTravelCommand>
    {
        public DeleteTravelCommandValidator()
        {
            RuleFor(x => x.TravelId)
                .NotEmpty().WithMessage("TravelId is required.");
        }
    }
}
