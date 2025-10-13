using Application.Common.Interfaces.Repositories;
using Application.Entities.Cities.Exceptions;
using Domain.Cities;
using LanguageExt;
using MediatR;

namespace Application.Entities.Cities.Commands;

public record DeleteCityCommand : IRequest<Either<CityException, City>>
{
    public required Guid CityId { get; init; }
}

public class DeleteCityCommandHandler(ICityRepository cityRepository)
    : IRequestHandler<DeleteCityCommand, Either<CityException, City>>
{
    public async Task<Either<CityException, City>> Handle(
        DeleteCityCommand request,
        CancellationToken cancellationToken)
    {
        var cityId = new CityId(request.CityId);
        var city = await cityRepository.GetByIdAsync(cityId, cancellationToken);

        return await city.MatchAsync(
            c => DeleteEntity(c, cancellationToken),
            () => new CityNotFoundException(cityId));
    }

    private async Task<Either<CityException, City>> DeleteEntity(
        City city,
        CancellationToken cancellationToken)
    {
        try
        {
            return await cityRepository.DeleteAsync(city, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledCityException(city.Id, ex);
        }
    }
}
