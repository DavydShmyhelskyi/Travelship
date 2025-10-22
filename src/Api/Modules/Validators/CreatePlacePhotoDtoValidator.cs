using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreatePlacePhotoDtoValidator : AbstractValidator<CreatePlacePhotoDto>
{
    public CreatePlacePhotoDtoValidator()
    {
        RuleFor(x => x.Photo)
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255);

        RuleFor(x => x.PlaceId)
            .NotEmpty();
    }
}
