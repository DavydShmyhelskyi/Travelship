using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Cities.Exceptions;
using Domain.Cities;
using Domain.Countries;
using LanguageExt;
using MediatR;

namespace Application.Entities.Cities.Commands;

public record CreateCityCommand : IRequest<Either<CityException, City>>
{
    public required string Title { get; init; }
    public required Guid CountryId { get; init; }
}

public class CreateCityCommandHandler(ICityRepository cityRepository,
    ICountryQueries countryQueries)
    : IRequestHandler<CreateCityCommand, Either<CityException, City>>
{
    public async Task<Either<CityException, City>> Handle(
        CreateCityCommand request,
        CancellationToken cancellationToken)
    {
        var countryId = new CountryId(request.CountryId);

        var country = await countryQueries.GetByIdAsync(countryId, cancellationToken);
        if (country.IsNone)
            return new CountryNotFoundForCityException(countryId);

        var existingCity = await cityRepository.GetByTitleAsync(request.Title, countryId, cancellationToken);

        return await existingCity.MatchAsync(
            c => new CityAlreadyExistException(c.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<CityException, City>> CreateEntity(
        CreateCityCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var city = City.New(request.Title, new CountryId(request.CountryId));
            var created = await cityRepository.AddAsync(city, cancellationToken);
            return created;
        }
        catch (Exception ex)
        {
            return new UnhandledCityException(CityId.Empty(), ex);
        }
    }
}
