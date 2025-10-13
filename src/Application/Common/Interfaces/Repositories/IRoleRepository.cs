using Domain.Roles;
using LanguageExt;
using LanguageExt.ClassInstances;

namespace Application.Common.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> AddAsync(Role entity, CancellationToken cancellationToken);
        Task<Role> UpdateAsync(Role entity, CancellationToken cancellationToken);
        Task<Role> DeleteAsync(Role entity, CancellationToken cancellationToken);
        Task<Option<Role>> GetByTitleAsync(string title, CancellationToken cancellationToken);
        Task<Option<Role>> GetByIdAsync(RoleId id, CancellationToken cancellationToken);
    }
}
