using Domain.Travels;

namespace Application.Common.Interfaces.Queries
{
    public interface ITravelQueries
    {
        Task<IReadOnlyList<Travel>> GetAllAsync(CancellationToken cancellationToken);
    }
}
