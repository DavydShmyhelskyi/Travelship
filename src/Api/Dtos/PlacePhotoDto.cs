using Domain.PlacePhotos;

namespace Api.Dtos;

public record PlacePhotoDto(Guid Id, byte[] Photo, string Description, bool IsShown, Guid PlaceId)
{
    public static PlacePhotoDto FromDomainModel(PlacePhoto placePhoto)
        => new(placePhoto.Id.Value, placePhoto.Photo, placePhoto.Description, placePhoto.IsShown, placePhoto.PlaceId.Value);
}

public record CreatePlacePhotoDto(byte[] Photo, string Description, Guid PlaceId);
