using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class UpdatePlacePhotoDtoValidator : AbstractValidator<UpdatePlacePhotoDto>
{
    public UpdatePlacePhotoDtoValidator()
    {
        RuleFor(x => x.Photo)
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255);

    }
}
