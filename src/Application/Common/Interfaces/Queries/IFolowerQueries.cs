using Domain.Followers;

namespace Application.Common.Interfaces.Queries
{
    public interface IFolowerQueries
    {
        Task<IReadOnlyList<Follower>> GetAllAsync(CancellationToken cancellationToken);
    }
}
