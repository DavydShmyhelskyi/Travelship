using Domain.Followers;
using Domain.Users;

namespace Application.Common.Interfaces.Queries
{
    public interface IFollowerQueries
    {
        Task<IReadOnlyList<Follower>> GetAllAsync(CancellationToken cancellationToken);
    }
}

