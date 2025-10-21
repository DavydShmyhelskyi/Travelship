using Domain.Cities;

namespace Api.Dtos;

public record CityDto(Guid Id, string Title, Guid CountryId)
{
    public static CityDto FromDomainModel(City city)
        => new(city.Id.Value, city.Title, city.CountryId.Value);
}

public record CreateCityDto(string Title, Guid CountryId);
