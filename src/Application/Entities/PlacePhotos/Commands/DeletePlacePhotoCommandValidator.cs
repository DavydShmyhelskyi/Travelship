

using FluentValidation;

namespace Application.Entities.PlacePhotos.Commands
{
    public class DeletePlacePhotoCommandValidator : AbstractValidator<DeletePlacePhotoCommand>
    {
        public DeletePlacePhotoCommandValidator()
        {
            RuleFor(x => x.PlacePhotoId)
                .NotEmpty().WithMessage("PlacePhotoId is required.");
        }
    }
}
