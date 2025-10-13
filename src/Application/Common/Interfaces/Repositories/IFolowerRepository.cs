using Domain.Followers;
using Domain.Users;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface IFolowerRepository
    {
        Task<Follower> AddAsync(Follower entity, CancellationToken cancellationToken);
        Task<Follower> DeleteAsync(Follower entity, CancellationToken cancellationToken);
        Task<Option<Follower>> GetAllFolloversAsync(UserId id, CancellationToken cancellationToken);
        Task<Option<Follower>> GetAllFollovingsAsync(UserId id, CancellationToken cancellationToken);
    }
}
