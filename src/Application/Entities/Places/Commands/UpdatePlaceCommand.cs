using Application.Common.Interfaces.Repositories;
using Application.Entities.Places.Exceptions;
using Domain.Places;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Entities.Places.Commands;

public record UpdatePlaceCommand : IRequest<Either<PlaceException, Place>>
{
    public required Guid PlaceId { get; init; }
    public required string Title { get; init; }
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
}

public class UpdatePlaceCommandHandler(IPlaceRepository placeRepository)
    : IRequestHandler<UpdatePlaceCommand, Either<PlaceException, Place>>
{
    public async Task<Either<PlaceException, Place>> Handle(
        UpdatePlaceCommand request,
        CancellationToken cancellationToken)
    {
        var placeId = new PlaceId(request.PlaceId);

        var place = await placeRepository.GetByIdAsync(placeId, cancellationToken);

        return await place.MatchAsync(
            p => CheckDuplicates(p.Id, request.Title, cancellationToken)
                .BindAsync(_ => UpdateEntity(request, p, cancellationToken)),
            () => new PlaceNotFoundException(placeId));
    }

    private async Task<Either<PlaceException, Place>> UpdateEntity(
        UpdatePlaceCommand request,
        Place place,
        CancellationToken cancellationToken)
    {
        try
        {
            place.Update(request.Title, request.Latitude, request.Longitude);
            return await placeRepository.UpdateAsync(place, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UnhandledPlaceException(place.Id, exception);
        }
    }

    private async Task<Either<PlaceException, Unit>> CheckDuplicates(
        PlaceId currentPlaceId,
        string title,
        CancellationToken cancellationToken)
    {
        var existing = await placeRepository.GetByTitleAsync(title, cancellationToken);

        return existing.Match<Either<PlaceException, Unit>>(
            p => p.Id.Equals(currentPlaceId) ? Unit.Default : new PlaceAlreadyExistException(p.Id),
            () => Unit.Default);
    }
}
