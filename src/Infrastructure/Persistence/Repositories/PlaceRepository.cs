using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Places;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PlaceRepository(ApplicationDbContext context)
    : IPlaceRepository, IPlaceQueries
{
    public async Task<Place> AddAsync(Place entity, CancellationToken cancellationToken)
    {
        await context.Places.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Place> UpdateAsync(Place entity, CancellationToken cancellationToken)
    {
        context.Places.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Place> DeleteAsync(Place entity, CancellationToken cancellationToken)
    {
        context.Places.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Option<Place>> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        var place = await context.Places
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => EF.Functions.ILike(x.Title, title),
                cancellationToken);

        return place is null ? Option<Place>.None : place;
    }


    public async Task<Option<Place>> GetByIdAsync(PlaceId id, CancellationToken cancellationToken)
    {
        var place = await context.Places
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return place is null ? Option<Place>.None : place;
    }

    public async Task<IReadOnlyCollection<Place>> GetByIdsAsync(
        IReadOnlyList<PlaceId> ids,
        CancellationToken cancellationToken)
    {
        return await context.Places
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Place>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Places
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
