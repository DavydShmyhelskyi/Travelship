using Domain.Places;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface IPlaceRepository
    {
        Task<Place> AddAsync(Place entity, CancellationToken cancellationToken);
        Task<Place> UpdateAsync(Place entity, CancellationToken cancellationToken);
        Task<Place> DeleteAsync(Place entity, CancellationToken cancellationToken);
        Task<Option<Place>> GetByTitleAsync(string title, CancellationToken cancellationToken);
        Task<Option<Place>> GetByIdAsync(PlaceId id, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<Place>> GetByIdsAsync(IReadOnlyList<PlaceId> ids, CancellationToken cancellationToken);
    }
}
