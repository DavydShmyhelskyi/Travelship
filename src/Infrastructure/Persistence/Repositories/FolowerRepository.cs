using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Followers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class FolowerRepository(ApplicationDbContext context) : IFollowerRepository, IFolowerQueries
{
    public async Task<Follower> AddAsync(Follower entity, CancellationToken cancellationToken)
    {
        await context.Followers.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<Follower>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Followers
            .Include(x => x.Follower)
            .Include(x => x.Followed)
            .ToListAsync(cancellationToken);
    }
}
