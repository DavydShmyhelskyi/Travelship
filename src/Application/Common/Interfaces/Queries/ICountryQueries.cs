using Domain.Countries;

namespace Application.Common.Interfaces.Queries
{
    public interface ICountryQueries
    {
        Task<IReadOnlyList<Country>> GetAllAsync(CancellationToken cancellationToken);
    }
}
