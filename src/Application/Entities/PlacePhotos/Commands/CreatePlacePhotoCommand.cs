using Application.Common.Interfaces.Repositories;
using Domain.PlacePhotos;
using MediatR;

namespace Application.Entities.PlacePhotos.Commands;

public class CreatePlacePhotoCommand : IRequest<PlacePhoto>
{
    public required byte[] Photo { get; set; }
    public string? Description { get; set; }
    public required Guid PlaceId { get; set; }
}
public class CreatePlacePhotoCommandHandler(IPlacePhotoRepository placePhotoRepository)
    : IRequestHandler<CreatePlacePhotoCommand, PlacePhoto>
{
    public async Task<PlacePhoto> Handle(CreatePlacePhotoCommand request, CancellationToken cancellationToken)
    {
        var placePhoto = PlacePhoto.New(
            request.Photo,
            request.Description ?? string.Empty,
            request.PlaceId
        );

        return await placePhotoRepository.AddAsync(placePhoto, cancellationToken);
    }
}