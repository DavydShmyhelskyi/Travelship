using Application.Common.Interfaces.Repositories;
using Application.Entities.PlacePhotos.Exceptions;
using Domain.PlacePhotos;
using LanguageExt;
using MediatR;

namespace Application.Entities.PlacePhotos.Commands;

public record UpdatePlacePhotoCommand : IRequest<Either<PlacePhotoException, PlacePhoto>>
{
    public required Guid PlacePhotoId { get; init; }
    public required byte[] Photo { get; init; }
    public required string Description { get; init; }
    public required bool IsShown { get; init; }
}

public class UpdatePlacePhotoCommandHandler(IPlacePhotoRepository placePhotoRepository)
    : IRequestHandler<UpdatePlacePhotoCommand, Either<PlacePhotoException, PlacePhoto>>
{
    public async Task<Either<PlacePhotoException, PlacePhoto>> Handle(
        UpdatePlacePhotoCommand request,
        CancellationToken cancellationToken)
    {
        var photoId = new PlacePhotoId(request.PlacePhotoId);
        var existing = await placePhotoRepository.GetByIdAsync(photoId, cancellationToken);

        return await existing.MatchAsync(
            p => UpdateEntity(request, p, cancellationToken),
            () => new PlacePhotoNotFoundException(photoId));
    }

    private async Task<Either<PlacePhotoException, PlacePhoto>> UpdateEntity(
        UpdatePlacePhotoCommand request,
        PlacePhoto placePhoto,
        CancellationToken cancellationToken)
    {
        try
        {
            placePhoto.Update(request.Photo, request.Description);
            placePhoto.ChangeVisibility(request.IsShown);

            var updated = await placePhotoRepository.UpdateAsync(placePhoto, cancellationToken);
            return updated;
        }
        catch (Exception ex)
        {
            return new UnhandledPlacePhotoException(placePhoto.Id, ex);
        }
    }
}
