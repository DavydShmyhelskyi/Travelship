using Domain.Places;

namespace Api.Dtos;

public record PlaceDto(Guid Id, string Title, double Latitude, double Longitude)
{
    public static PlaceDto FromDomainModel(Place place)
        => new(place.Id.Value, place.Title, place.Latitude, place.Longitude);
}

public record CreatePlaceDto(string Title, double Latitude, double Longitude);

public record UpdatePlaceDto(Guid Id, string Title, double Latitude, double Longitude);