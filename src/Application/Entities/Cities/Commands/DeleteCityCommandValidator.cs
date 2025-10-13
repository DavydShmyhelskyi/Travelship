using FluentValidation;

namespace Application.Entities.Cities.Commands;

public class DeleteCityCommandValidator : AbstractValidator<DeleteCityCommand>
{
    public DeleteCityCommandValidator()
    {
        RuleFor(x => x.CityId).NotEmpty();
    }
}
