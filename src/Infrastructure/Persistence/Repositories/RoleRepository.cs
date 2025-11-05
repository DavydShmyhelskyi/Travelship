using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Roles;
using LanguageExt;
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
        return await context.Roles.AsNoTracking().ToListAsync(cancellationToken);
    }
    public async Task<Role> UpdateAsync(Role entity, CancellationToken cancellationToken)
    {
        context.Roles.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }
    public async Task<Role> DeleteAsync(Role entity, CancellationToken cancellationToken)
    {
        context.Roles.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

public async Task<Option<Role>> GetByTitleAsync(string title, CancellationToken cancellationToken)
{
    var role = await context.Roles
        .AsNoTracking()
        .FirstOrDefaultAsync(
            r => EF.Functions.ILike(r.Title, title),
            cancellationToken);

    return role ?? Option<Role>.None;
}

public async Task<Option<Role>> GetByIdAsync(RoleId id, CancellationToken cancellationToken)
    {
        var role = await context.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        return role ?? Option<Role>.None;
    }

}
