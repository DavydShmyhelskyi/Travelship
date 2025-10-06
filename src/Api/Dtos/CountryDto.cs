using Domain.Countries;

namespace Api.Dtos;

public record CountryDto(Guid Id, string Title)
{
    public static CountryDto FromDomainModel(Country country)
        => new(country.Id, country.Title);
}

public record CreateCountryDto(string Title);
