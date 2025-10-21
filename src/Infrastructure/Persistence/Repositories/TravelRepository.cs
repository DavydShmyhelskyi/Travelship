using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Travels;
using Domain.Users;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TravelRepository(ApplicationDbContext context)
    : ITravelRepository, ITravelQueries
{
    public async Task<Travel> AddAsync(Travel entity, CancellationToken cancellationToken)
    {
        await context.Travels.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Travel> UpdateAsync(Travel entity, CancellationToken cancellationToken)
    {
        context.Travels.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Travel> DeleteAsync(Travel entity, CancellationToken cancellationToken)
    {
        context.Travels.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Option<Travel>> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        var travel = await context.Travels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Title == title, cancellationToken);

        return travel is null ? Option<Travel>.None : travel;
    }

    public async Task<Option<Travel>> GetByIdAsync(TravelId id, CancellationToken cancellationToken)
    {
        var travel = await context.Travels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return travel is null ? Option<Travel>.None : travel;
    }

    public async Task<Option<Travel>> GetByUserAsync(UserId userId, CancellationToken cancellationToken)
    {
        var travel = await context.Travels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        return travel is null ? Option<Travel>.None : travel;
    }

    public async Task<IReadOnlyList<Travel>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Travels
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
