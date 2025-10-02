using Domain.Roles;

namespace Application.Common.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> AddAsync(Role entity, CancellationToken cancellationToken);
    }
}
