using Domain.Cities;

namespace Application.Common.Interfaces.Queries
{
    public interface ICityQueries
    {
        Task<IReadOnlyList<City>> GetAllAsync(CancellationToken cancellationToken);
    }
}
