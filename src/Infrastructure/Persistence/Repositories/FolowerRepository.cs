using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Followers;
using Domain.Users;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Infrastructure.Persistence.Repositories;

public class FollowerRepository(ApplicationDbContext context)
    : IFollowerRepository, IFollowerQueries
{
    public async Task<Follower> AddAsync(Follower entity, CancellationToken cancellationToken)
    {
        await context.Followers.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Follower> DeleteAsync(Follower entity, CancellationToken cancellationToken)
    {
        context.Followers.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Option<Follower>> GetByIdsAsync(
    UserId followerId,
    UserId followedId,
    CancellationToken cancellationToken)
    {
        var follower = await context.Followers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.FollowerUserId == followerId && x.FollowedUserId == followedId,
                cancellationToken
            );

        return follower is null ? Option<Follower>.None : follower;
    }


    public async Task<IReadOnlyList<Follower>> GetFollowersAsync(UserId userId, CancellationToken cancellationToken)
    {
        return await context.Followers
            .AsNoTracking()
            .Where(x => x.FollowedUserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Follower>> GetFollowingsAsync(UserId userId, CancellationToken cancellationToken)
    {
        return await context.Followers
            .AsNoTracking()
            .Where(x => x.FollowerUserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Follower>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Followers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
