using Domain.Countries;

namespace Application.Entities.Countries.Exceptions;

public abstract class CountryException(CountryId countryId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public CountryId CountryId { get; } = countryId;
}

public class CountryAlreadyExistException(CountryId countryId) : CountryException(countryId, $"Country already exists under id {countryId}");

public class CountryNotFoundException(CountryId countryId) : CountryException(countryId, $"Country not found under id {countryId}");

public class UnhandledCountryException(CountryId countryId, Exception? innerException = null)
    : CountryException(countryId, "Unexpected error occurred", innerException);