using Application.Common.Interfaces.Repositories;
using Application.Entities.Places.Exceptions;
using Domain.Places;
using LanguageExt;
using MediatR;

namespace Application.Entities.Places.Commands;

public record DeletePlaceCommand : IRequest<Either<PlaceException, Place>>
{
    public required Guid PlaceId { get; init; }
}

public class DeletePlaceCommandHandler(IPlaceRepository placeRepository)
    : IRequestHandler<DeletePlaceCommand, Either<PlaceException, Place>>
{
    public async Task<Either<PlaceException, Place>> Handle(
        DeletePlaceCommand request,
        CancellationToken cancellationToken)
    {
        var placeId = new PlaceId(request.PlaceId);
        var place = await placeRepository.GetByIdAsync(placeId, cancellationToken);

        return await place.MatchAsync(
            p => DeleteEntity(p, cancellationToken),
            () => new PlaceNotFoundException(placeId));
    }

    private async Task<Either<PlaceException, Place>> DeleteEntity(
        Place place,
        CancellationToken cancellationToken)
    {
        try
        {
            return await placeRepository.DeleteAsync(place, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UnhandledPlaceException(place.Id, exception);
        }
    }
}
