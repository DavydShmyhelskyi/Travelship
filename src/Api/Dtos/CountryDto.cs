using Domain.Countries;

namespace Api.Dtos;

public record CountryDto(Guid Id, string Title)
{
    public static CountryDto FromDomainModel(Country country)
        => new(country.Id.Value, country.Title);
}

public record CreateCountryDto(string Title);

public record UpdateCountryDto(Guid Id, string Title);
