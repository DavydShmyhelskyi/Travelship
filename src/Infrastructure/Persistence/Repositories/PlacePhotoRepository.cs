using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.PlacePhotos;
using LanguageExt;
using MediatR.NotificationPublishers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PlacePhotoRepository(ApplicationDbContext context) : IPlacePhotoRepository, IPlacePhotoQueries
{
    public async Task<IReadOnlyList<PlacePhoto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.PlacePhotos
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task<PlacePhoto> AddAsync(PlacePhoto entity, CancellationToken cancellationToken)
    {
        await context.PlacePhotos.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<PlacePhoto> UpdateAsync(PlacePhoto entity, CancellationToken cancellationToken)
    {
        context.PlacePhotos.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<PlacePhoto> DeleteAsync(PlacePhoto entity, CancellationToken cancellationToken)
    {
        context.PlacePhotos.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Option<PlacePhoto>> GetByDescriptionAsync(string description, CancellationToken cancellationToken)
    {
        var photo = await context.PlacePhotos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Description == description, cancellationToken);

        return photo is null ? Option<PlacePhoto>.None : photo;
    }

    public async Task<Option<PlacePhoto>> GetByIdAsync(PlacePhotoId id, CancellationToken cancellationToken)
    {
        var photo = await context.PlacePhotos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return photo is null ? Option<PlacePhoto>.None : photo;
    }
}
