using Application.Common.Interfaces.Repositories;
using Application.Entities.Places.Exceptions;
using Domain.Places;
using LanguageExt;
using MediatR;

namespace Application.Entities.Places.Commands;

public record CreatePlaceCommand : IRequest<Either<PlaceException, Place>>
{
    public required string Title { get; init; }
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
}
public class CreatePlaceCommandHandler(IPlaceRepository placeRepository)
    : IRequestHandler<CreatePlaceCommand, Either<PlaceException, Place>>
{
    public async Task<Either<PlaceException, Place>> Handle(
        CreatePlaceCommand request,
        CancellationToken cancellationToken)
    {
        var existingPlace = await placeRepository.GetByTitleAsync(request.Title, cancellationToken);

        return await existingPlace.MatchAsync(
            p => new PlaceAlreadyExistException(p.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<PlaceException, Place>> CreateEntity(
        CreatePlaceCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var place = Place.New(request.Title, request.Latitude, request.Longitude);
            var created = await placeRepository.AddAsync(place, cancellationToken);
            return created;
        }
        catch (Exception ex)
        {
            return new UnhandledPlaceException(PlaceId.Empty(), ex);
        }
    }
}