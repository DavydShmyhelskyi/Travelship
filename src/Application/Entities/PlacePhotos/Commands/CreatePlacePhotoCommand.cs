using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.PlacePhotos.Exceptions;
using Domain.PlacePhotos;
using Domain.Places;
using LanguageExt;
using MediatR;

namespace Application.Entities.PlacePhotos.Commands;

public record CreatePlacePhotoCommand : IRequest<Either<PlacePhotoException, PlacePhoto>>
{
    public required byte[] Photo { get; init; }
    public required string Description { get; init; }
    public required Guid PlaceId { get; init; }
}
public class CreatePlacePhotoCommandHandler(
    IPlacePhotoRepository placePhotoRepository,
    IPlaceRepository placeRepository)
    : IRequestHandler<CreatePlacePhotoCommand, Either<PlacePhotoException, PlacePhoto>>
{
    public async Task<Either<PlacePhotoException, PlacePhoto>> Handle(
        CreatePlacePhotoCommand request,
        CancellationToken cancellationToken)
    {
        var placeId = new PlaceId(request.PlaceId);

        var place = await placeRepository.GetByIdAsync(placeId, cancellationToken);
        if (place.IsNone)
            return new PlaceNotFoundForPhotoException(placeId);

        return await CreateEntity(request, placeId, cancellationToken);
    }

    private async Task<Either<PlacePhotoException, PlacePhoto>> CreateEntity(
        CreatePlacePhotoCommand request,
        PlaceId placeId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = PlacePhoto.New(request.Photo, request.Description, placeId);
            var created = await placePhotoRepository.AddAsync(entity, cancellationToken);
            return created;
        }
        catch (Exception ex)
        {
            return new UnhandledPlacePhotoException(PlacePhotoId.Empty(), ex);
        }
    }
}

