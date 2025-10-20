using Domain.Places;
using Domain.Travels;

namespace Application.Common.Interfaces.Repositories
{
    public interface ITravelPlaceRepository
    {
        Task<IReadOnlyList<TravelPlace>> AddRangeAsync(
        IReadOnlyList<TravelPlace> entities,
        CancellationToken cancellationToken);

        Task<IReadOnlyList<TravelPlace>> RemoveRangeAsync(
            IReadOnlyList<TravelPlace> entities,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<TravelPlace>> GetByTravelIdAsync(
            TravelId travelId,
            CancellationToken cancellationToken);
    }
}
