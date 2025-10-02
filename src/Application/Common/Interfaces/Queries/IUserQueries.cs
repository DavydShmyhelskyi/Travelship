using Domain.Users;

namespace Application.Common.Interfaces.Queries
{
    public interface IUserQueries
    {
        Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);
    }
}
