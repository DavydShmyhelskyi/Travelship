using Domain.Countries;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface ICountryQueries
    {
        Task<IReadOnlyList<Country>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<Country>> GetByIdAsync(CountryId countryId, CancellationToken cancellationToken);
        Task<Option<Country>> GetByTitleAsync(string title, CancellationToken cancellationToken);
    }
}
