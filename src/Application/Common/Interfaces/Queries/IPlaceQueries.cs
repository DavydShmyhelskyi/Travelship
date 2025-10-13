using Domain.Places;

namespace Application.Common.Interfaces.Queries
{
    public interface IPlaceQueries
    {
        Task<IReadOnlyList<Place>> GetAllAsync(CancellationToken cancellationToken);
    }
}
