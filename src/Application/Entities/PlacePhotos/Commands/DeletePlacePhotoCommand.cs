using Application.Common.Interfaces.Repositories;
using Application.Entities.PlacePhotos.Exceptions;
using Domain.PlacePhotos;
using LanguageExt;
using MediatR;

namespace Application.Entities.PlacePhotos.Commands;

public record DeletePlacePhotoCommand : IRequest<Either<PlacePhotoException, PlacePhoto>>
{
    public required Guid PlacePhotoId { get; init; }
}

public class DeletePlacePhotoCommandHandler(IPlacePhotoRepository placePhotoRepository)
    : IRequestHandler<DeletePlacePhotoCommand, Either<PlacePhotoException, PlacePhoto>>
{
    public async Task<Either<PlacePhotoException, PlacePhoto>> Handle(
        DeletePlacePhotoCommand request,
        CancellationToken cancellationToken)
    {
        var photoId = new PlacePhotoId(request.PlacePhotoId);
        var existing = await placePhotoRepository.GetByIdAsync(photoId, cancellationToken);

        return await existing.MatchAsync(
            p => DeleteEntity(p, cancellationToken),
            () => new PlacePhotoNotFoundException(photoId));
    }

    private async Task<Either<PlacePhotoException, PlacePhoto>> DeleteEntity(
        PlacePhoto placePhoto,
        CancellationToken cancellationToken)
    {
        try
        {
            return await placePhotoRepository.DeleteAsync(placePhoto, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledPlacePhotoException(placePhoto.Id, ex);
        }
    }
}
