using FluentValidation;

namespace Application.Entities.PlacePhotos.Commands;

public class CreatePlacePhotoCommandValidator : AbstractValidator<CreatePlacePhotoCommand>
{
    public CreatePlacePhotoCommandValidator()
    {
        RuleFor(x => x.Photo)
            .NotEmpty().WithMessage("Photo is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must be less than 500 characters.");

        RuleFor(x => x.PlaceId)
            .NotEmpty().WithMessage("PlaceId is required.");
    }
}
