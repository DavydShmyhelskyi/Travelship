using Domain.Travels;
using Domain.Users;

namespace Application.Entities.Travels.Exceptions;

public abstract class TravelException(TravelId travelId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public TravelId TravelId { get; } = travelId;
}

public class TravelAlreadyExistsException(TravelId travelId)
    : TravelException(travelId, $"Travel already exists under id {travelId}");

public class TravelNotFoundException(TravelId travelId)
    : TravelException(travelId, $"Travel not found under id {travelId}");

public class UnhandledTravelException(TravelId travelId, Exception? innerException = null)
    : TravelException(travelId, "Unexpected error occurred", innerException);

public class MembersNotFoundException(TravelId travelId, IReadOnlyList<Guid> memberIds)
    : TravelException(travelId, $"Members of travel not found: {string.Join(", ", memberIds)}");

public class AccessDeniedToTravelException(TravelId travelId) 
    : TravelException(travelId, $"You do not have permission to delete travel '{travelId}'.");

public class PlacesNotFoundException(TravelId travelId, List<Guid> missing)
    : TravelException(travelId, $"Places '{missing}' not found for travel '{travelId}'.");