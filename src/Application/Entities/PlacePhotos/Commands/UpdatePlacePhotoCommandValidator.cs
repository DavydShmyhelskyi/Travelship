using FluentValidation;

namespace Application.Entities.PlacePhotos.Commands;

public class UpdatePlacePhotoCommandValidator : AbstractValidator<UpdatePlacePhotoCommand>
{
    public UpdatePlacePhotoCommandValidator()
    {
        RuleFor(x => x.Photo)
            .NotEmpty().WithMessage("Photo is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MinimumLength(3).WithMessage("Description must be at least 10 characters.")
            .MaximumLength(500).WithMessage("Description must be less than 500 characters.");

        RuleFor(x => x.PlacePhotoId)
            .NotEmpty().WithMessage("PlacePhotoId is required.");
    }
}
