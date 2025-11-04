using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators
{
    public class UpdateCountryDtoValidator : AbstractValidator<UpdateCountryDto>
    {
        public UpdateCountryDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);
        }
    }
}
