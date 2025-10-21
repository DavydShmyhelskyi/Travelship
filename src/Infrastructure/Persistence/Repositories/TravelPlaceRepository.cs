using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Places;
using Domain.Travels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TravelPlaceRepository(ApplicationDbContext context)
    : ITravelPlaceRepository, ITravelPlaceQueries
{
    public async Task<IReadOnlyList<TravelPlace>> AddRangeAsync(
        IReadOnlyList<TravelPlace> entities,
        CancellationToken cancellationToken)
    {
        await context.TravelPlaces.AddRangeAsync(entities, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entities;
    }

    public async Task<IReadOnlyList<TravelPlace>> RemoveRangeAsync(
        IReadOnlyList<TravelPlace> entities,
        CancellationToken cancellationToken)
    {
        context.TravelPlaces.RemoveRange(entities);
        await context.SaveChangesAsync(cancellationToken);
        return entities;
    }

    public async Task<IReadOnlyList<TravelPlace>> GetByTravelIdAsync(
        TravelId travelId,
        CancellationToken cancellationToken)
    {
        return await context.TravelPlaces
            .AsNoTracking()
            .Where(x => x.TravelId == travelId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TravelPlace>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.TravelPlaces
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
