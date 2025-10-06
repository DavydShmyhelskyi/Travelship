using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Folowers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class FolowerRepository(ApplicationDbContext context) : IFolowerRepository, IFolowerQueries
{
    public async Task<Folower> AddAsync(Folower entity, CancellationToken cancellationToken)
    {
        await context.Followers.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<Folower>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Followers
            .Include(x => x.Follower)
            .Include(x => x.Followed)
            .ToListAsync(cancellationToken);
    }
}
