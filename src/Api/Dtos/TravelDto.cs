using Domain.Places;
using Domain.Travels;
using Domain.Users;

namespace Api.Dtos;

public record TravelDto(Guid Id, string Title, DateTime StartDate, DateTime EndDate, string Description, byte[]? Image, bool IsDone, Guid UserId)
{
    public static TravelDto FromDomainModel(Travel travel)
        => new(travel.Id.Value, travel.Title, travel.StartDate, travel.EndDate, travel.Description, travel.Image, travel.IsDone, travel.UserId.Value);
}

public record CreateTravelDto(string Title, DateTime StartDate, DateTime EndDate, string Description, byte[]? Image, Guid UserId);

// UserTravel DTOs
public record UserTravelDto(Guid UserId, Guid TravelId)
{
    public static UserTravelDto FromDomainModel(UserTravel userTravel)
        => new(userTravel.UserId.Value, userTravel.TravelId.Value);
}

// TravelPlace DTOs
public record TravelPlaceDto(Guid TravelId, Guid PlaceId)
{
    public static TravelPlaceDto FromDomainModel(TravelPlace travelPlace)
        => new(travelPlace.TravelId.Value, travelPlace.PlaceId.Value);
}