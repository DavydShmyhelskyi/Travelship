using Domain.Places;
using Domain.Travels;
using Domain.Users;

namespace Api.Dtos;

// Головна DTO для відображення подорожі
public record TravelDto(
    Guid Id,
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    string Description,
    byte[]? Image,
    bool IsDone,
    Guid UserId,
    IReadOnlyList<Guid> Members,
    IReadOnlyList<Guid> Places)
{
    public static TravelDto FromDomainModel(Travel travel)
        => new(
            travel.Id.Value,
            travel.Title,
            travel.StartDate,
            travel.EndDate,
            travel.Description,
            travel.Image,
            travel.IsDone,
            travel.UserId.Value,
            travel.Members?.Select(m => m.UserId.Value).ToList() ?? new List<Guid>(),
            travel.Places?.Select(p => p.PlaceId.Value).ToList() ?? new List<Guid>());
}

public record CreateTravelDto(
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    string Description,
    byte[]? Image,
    Guid UserId,
    IReadOnlyList<Guid> Members,
    IReadOnlyList<Guid> Places);

public record UpdateTravelDto(
    Guid Id,
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    string Description,
    byte[]? Image,
    bool IsDone,
    IReadOnlyList<Guid> Members,
    IReadOnlyList<Guid> Places);

// UserTravel DTO
public record UserTravelDto(Guid UserId, Guid TravelId)
{
    public static UserTravelDto FromDomainModel(UserTravel userTravel)
        => new(userTravel.UserId.Value, userTravel.TravelId.Value);
}

// TravelPlace DTO
public record TravelPlaceDto(Guid TravelId, Guid PlaceId)
{
    public static TravelPlaceDto FromDomainModel(TravelPlace travelPlace)
        => new(travelPlace.TravelId.Value, travelPlace.PlaceId.Value);
}
