using Application.Entities.Places.Commands;
using FluentValidation;

namespace Application.Entities.Roles.Commands
{
    public class DeletePlaceCommandValidator :  AbstractValidator<DeletePlaceCommand>
    {
        public DeletePlaceCommandValidator()
        {
            RuleFor(x => x.PlaceId)
                .NotEmpty().WithMessage("PlaceId is required.");
        }
    }
}
