using Api.Dtos;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface ICountriesControllerService
    {
        Task<Option<CountryDto>> Get(Guid countryId, CancellationToken cancellationToken);
    }
}
