using Domain.Travels;

namespace Api.Dtos;

public record TravelDto(Guid Id, string Title, DateTime StartDate, DateTime EndDate, string Description, byte[]? Image, bool IsDone, Guid UserId)
{
    public static TravelDto FromDomainModel(Travel travel)
        => new(travel.Id, travel.Title, travel.StartDate, travel.EndDate, travel.Description, travel.Image, travel.IsDone, travel.UserId);
}

public record CreateTravelDto(string Title, DateTime StartDate, DateTime EndDate, string Description, byte[]? Image, bool IsDone, Guid UserId);
