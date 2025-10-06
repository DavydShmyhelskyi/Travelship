using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Places;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PlaceRepository(ApplicationDbContext context) : IPlaceRepository, IPlaceQueries
{
    public async Task<Place> AddAsync(Place entity, CancellationToken cancellationToken)
    {
        await context.Places.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<Place>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Places
            .Include(x => x.PlacePhotos)
            .ToListAsync(cancellationToken);
    }
}
