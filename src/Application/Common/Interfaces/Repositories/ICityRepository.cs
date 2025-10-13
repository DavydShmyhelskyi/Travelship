using Domain.Cities;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface ICityRepository
    {
        Task<City> AddAsync(City entity, CancellationToken cancellationToken);
        Task<City> UpdateAsync(City entity, CancellationToken cancellationToken);
        Task<City> DeleteAsync(City entity, CancellationToken cancellationToken);
        Task<Option<City>> GetByTitleAsync(string title, CancellationToken cancellationToken);
        Task<Option<City>> GetByIdAsync(CityId id, CancellationToken cancellationToken);
    }
}
