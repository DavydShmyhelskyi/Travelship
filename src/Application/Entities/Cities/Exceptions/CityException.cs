using Domain.Cities;
using Domain.Countries;

namespace Application.Entities.Cities.Exceptions;

public abstract class CityException(CityId cityId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public CityId CityId { get; } = cityId;
}

public class CityAlreadyExistException(CityId cityId)
    : CityException(cityId, $"City already exists under id {cityId}");

public class CityNotFoundException(CityId cityId)
    : CityException(cityId, $"City not found under id {cityId}");

public class CountryNotFoundForCityException(CountryId countryId)
    : CityException(CityId.Empty(), $"Country with id {countryId} does not exist");

public class UnhandledCityException(CityId cityId, Exception? innerException = null)
    : CityException(cityId, "Unexpected error occurred", innerException);
