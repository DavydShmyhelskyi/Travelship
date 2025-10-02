using Domain.Users;

namespace Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User entity, CancellationToken cancellationToken);
    }
}
