using Domain.Users;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User entity, CancellationToken cancellationToken);
        Task<User> UpdateAsync(User entity, CancellationToken cancellationToken);
        Task<User> DeleteAsync(User entity, CancellationToken cancellationToken);
        Task<Option<User>> GetByNickNameAsync(string nickname, CancellationToken cancellationToken);
        Task<Option<User>> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Option<User>> GetByIdAsync(UserId id, CancellationToken cancellationToken);
        Task<IReadOnlyList<User>> GetByIdsAsync(IReadOnlyList<UserId> userIds, CancellationToken cancellationToken);

    }
}
