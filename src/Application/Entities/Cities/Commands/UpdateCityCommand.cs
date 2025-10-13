using Application.Common.Interfaces.Repositories;
using Application.Entities.Cities.Exceptions;
using Domain.Cities;
using Domain.Countries;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Entities.Cities.Commands;

public record UpdateCityCommand : IRequest<Either<CityException, City>>
{
    public required Guid CityId { get; init; }
    public required string Title { get; init; }
    public required Guid CountryId { get; init; }
}

public class UpdateCityCommandHandler(ICityRepository cityRepository)
    : IRequestHandler<UpdateCityCommand, Either<CityException, City>>
{
    public async Task<Either<CityException, City>> Handle(
        UpdateCityCommand request,
        CancellationToken cancellationToken)
    {
        var cityId = new CityId(request.CityId);
        var countryId = new CountryId(request.CountryId);

        var city = await cityRepository.GetByIdAsync(cityId, cancellationToken);

        return await city.MatchAsync(
            c => CheckDuplicates(c.Id, request.Title, countryId, cancellationToken)
                .BindAsync(_ => UpdateEntity(request, c, cancellationToken)),
            () => new CityNotFoundException(cityId));
    }

    private async Task<Either<CityException, City>> UpdateEntity(
        UpdateCityCommand request,
        City city,
        CancellationToken cancellationToken)
    {
        try
        {
            city.ChangeTitle(request.Title);
            return await cityRepository.UpdateAsync(city, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledCityException(city.Id, ex);
        }
    }

    private async Task<Either<CityException, Unit>> CheckDuplicates(
        CityId currentCityId,
        string title,
        CountryId countryId,
        CancellationToken cancellationToken)
    {
        var existing = await cityRepository.GetByTitleAsync(title, countryId, cancellationToken);

        return existing.Match<Either<CityException, Unit>>(
            c => c.Id.Equals(currentCityId) ? Unit.Default : new CityAlreadyExistException(c.Id),
            () => Unit.Default);
    }
}
