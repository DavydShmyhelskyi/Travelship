using Domain.Users;

namespace Application.Common.Interfaces.Queries
{
    public interface IUserTravelQueries
    {
        Task<IReadOnlyList<UserTravel>> GetAllAsync(CancellationToken cancellationToken);
    }
}
