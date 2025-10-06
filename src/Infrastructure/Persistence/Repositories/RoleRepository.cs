using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Roles;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RoleRepository(ApplicationDbContext context) : IRoleRepository, IRoleQueries
{
    public async Task<Role> AddAsync(Role entity, CancellationToken cancellationToken)
    {
        await context.Roles.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Roles.ToListAsync(cancellationToken);
    }
}
