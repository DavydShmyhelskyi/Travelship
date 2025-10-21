using Domain.Followers;
using Domain.Users;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface IFollowerRepository
    {
        Task<Follower> AddAsync(Follower entity, CancellationToken cancellationToken);
        Task<Follower> DeleteAsync(Follower entity, CancellationToken cancellationToken);
        Task<Option<Follower>> GetByIdsAsync(UserId followerId, UserId followedId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Follower>> GetFollowersAsync(UserId userId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Follower>> GetFollowingsAsync(UserId userId, CancellationToken cancellationToken);
    }
}
