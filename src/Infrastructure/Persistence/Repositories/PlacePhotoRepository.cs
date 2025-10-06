using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.PlacePhotos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PlacePhotoRepository(ApplicationDbContext context) : IPlacePhotoRepository, IPlacePhotoQueries
{
    public async Task<PlacePhoto> AddAsync(PlacePhoto entity, CancellationToken cancellationToken)
    {
        await context.PlacePhotos.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<PlacePhoto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.PlacePhotos
            .Include(x => x.Place)
            .ToListAsync(cancellationToken);
    }
}
