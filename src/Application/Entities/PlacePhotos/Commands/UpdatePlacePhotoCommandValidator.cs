using FluentValidation;

namespace Application.Entities.PlacePhotos.Commands;

public class UpdatePlacePhotoCommandValidator : AbstractValidator<UpdatePlacePhotoCommand>
{
    public UpdatePlacePhotoCommandValidator()
    {
        RuleFor(x => x.Photo)
            .NotEmpty().WithMessage("Photo is required.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must be less than 500 characters.");
    }
}
