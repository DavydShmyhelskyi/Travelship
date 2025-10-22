using Domain.PlacePhotos;

namespace Api.Dtos;

public record PlacePhotoDto(Guid Id, byte[] Photo, string Description, bool IsShown, Guid PlaceId)
{
    public static PlacePhotoDto FromDomainModel(PlacePhoto photo)
        => new(photo.Id.Value, photo.Photo, photo.Description, photo.IsShown, photo.PlaceId.Value);
}

public record CreatePlacePhotoDto(byte[] Photo, string Description, Guid PlaceId);
