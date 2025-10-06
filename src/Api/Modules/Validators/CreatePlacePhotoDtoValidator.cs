using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreatePlacePhotoDtoValidator : AbstractValidator<CreatePlacePhotoDto>
{
    public CreatePlacePhotoDtoValidator()
    {
        RuleFor(x => x.Photo)
            .NotNull()
            .Must(p => p.Length > 0).WithMessage("Photo cannot be empty");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.PlaceId)
            .NotEmpty();
    }
}
