using Domain.PlacePhotos;
using Domain.Places;

namespace Application.Entities.PlacePhotos.Exceptions;

public abstract class PlacePhotoException(PlacePhotoId placePhotoId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public PlacePhotoId PlacePhotoId { get; } = placePhotoId;
}

public class PlacePhotoNotFoundException(PlacePhotoId placePhotoId)
    : PlacePhotoException(placePhotoId, $"Place photo not found under id {placePhotoId}");

public class PlacePhotoAlreadyExistException(PlacePhotoId placePhotoId)
    : PlacePhotoException(placePhotoId, $"Place photo already exists under id {placePhotoId}");

public class UnhandledPlacePhotoException(PlacePhotoId placePhotoId, Exception? innerException = null)
    : PlacePhotoException(placePhotoId, "Unexpected error occurred", innerException);

public class PlaceNotFoundForPhotoException(PlaceId placeId)
    : PlacePhotoException(PlacePhotoId.Empty(), $"Place not found for photo. PlaceId: {placeId}");
