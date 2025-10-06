using Domain.PlacePhotos;

namespace Api.Dtos;

public record PlacePhotoDto(Guid Id, byte[] Photo, string Description, bool IsShown, Guid PlaceId)
{
    public static PlacePhotoDto FromDomainModel(PlacePhoto placePhoto)
        => new(placePhoto.Id, placePhoto.Photo, placePhoto.Description, placePhoto.IsShown, placePhoto.PlaceId);
}

public record CreatePlacePhotoDto(byte[] Photo, string Description, Guid PlaceId);
