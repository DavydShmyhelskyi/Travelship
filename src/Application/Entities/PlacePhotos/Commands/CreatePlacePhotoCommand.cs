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

public class CreatePlacePhotoCommandHandler(IPlacePhotoRepository placePhotoRepository)
    : IRequestHandler<CreatePlacePhotoCommand, Either<PlacePhotoException, PlacePhoto>>
{
    public async Task<Either<PlacePhotoException, PlacePhoto>> Handle(
        CreatePlacePhotoCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var placePhoto = PlacePhoto.New(
                request.Photo,
                request.Description,
                new PlaceId(request.PlaceId));

            var created = await placePhotoRepository.AddAsync(placePhoto, cancellationToken);
            return created;
        }
        catch (Exception ex)
        {
            return new UnhandledPlacePhotoException(PlacePhotoId.Empty(), ex);
        }
    }
}
