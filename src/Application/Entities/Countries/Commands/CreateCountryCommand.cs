using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Countries.Exceptions;
using Domain.Countries;
using LanguageExt;
using MediatR;

namespace Application.Entities.Countries.Commands;

public record CreateCountryCommand : IRequest<Either<CountryException, Country>>
{
    public required string Title { get; init; }
}

public class CreateCountryCommandHandler(ICountryRepository countryRepository, ICountryQueries countryQueries)
    : IRequestHandler<CreateCountryCommand, Either<CountryException, Country>>
{
    public async Task<Either<CountryException, Country>> Handle(
        CreateCountryCommand request,
        CancellationToken cancellationToken)
    {
        var existingCountry = await countryQueries.GetByTitleAsync(request.Title, cancellationToken);

        return await existingCountry.MatchAsync(
            c => new CountryAlreadyExistException(c.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<CountryException, Country>> CreateEntity(
        CreateCountryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var country = Country.New(request.Title);
            var created = await countryRepository.AddAsync(country, cancellationToken);
            return created;
        }
        catch (Exception ex)
        {
            return new UnhandledCountryException(CountryId.Empty(), ex);
        }
    }
}
