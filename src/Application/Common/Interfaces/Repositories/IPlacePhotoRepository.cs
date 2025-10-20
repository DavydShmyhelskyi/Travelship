using Domain.PlacePhotos;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface IPlacePhotoRepository
    {
        Task<PlacePhoto> AddAsync(PlacePhoto entity, CancellationToken cancellationToken);
        Task<PlacePhoto> UpdateAsync(PlacePhoto entity, CancellationToken cancellationToken);
        Task<PlacePhoto> DeleteAsync(PlacePhoto entity, CancellationToken cancellationToken);
        Task<Option<PlacePhoto>> GetByDescriptionAsync(string description, CancellationToken cancellationToken);
        Task<Option<PlacePhoto>> GetByIdAsync(PlacePhotoId id, CancellationToken cancellationToken);
    }
}
