using Domain.Countries;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface ICountryRepository
    {
        Task<Country> AddAsync(Country entity, CancellationToken cancellationToken);
        Task<Country> UpdateAsync(Country entity, CancellationToken cancellationToken);
        Task<Country> DeleteAsync(Country entity, CancellationToken cancellationToken);
    }
}
