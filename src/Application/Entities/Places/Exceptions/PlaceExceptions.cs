using Domain.Places;
namespace Application.Entities.Places.Exceptions;

public abstract class PlaceException(PlaceId placeId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public PlaceId PlaceId { get; } = placeId;
}

public class PlaceAlreadyExistException(PlaceId placeId) : PlaceException(placeId, $"Country already exists under id {placeId}");

public class PlaceNotFoundException(PlaceId placeId) : PlaceException(placeId, $"Country not found under id {placeId}");

public class UnhandledPlaceException(PlaceId placeId, Exception? innerException = null)
    : PlaceException(placeId, "Unexpected error occurred", innerException);