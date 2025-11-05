using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators
{
    public class UpdateCityDtoValidator : AbstractValidator<UpdateCityDto>
    {
        public UpdateCityDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(3);
            RuleFor(x => x.CountryId)
                .NotEmpty();
        }
    }
}
