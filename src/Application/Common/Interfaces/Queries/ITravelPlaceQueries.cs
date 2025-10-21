using Domain.Places;

namespace Application.Common.Interfaces.Queries
{
    public interface ITravelPlaceQueries
    {
        Task<IReadOnlyList<TravelPlace>> GetAllAsync(CancellationToken cancellationToken);
    }
}
