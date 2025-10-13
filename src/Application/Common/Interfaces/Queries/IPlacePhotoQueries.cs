using Domain.PlacePhotos;

namespace Application.Common.Interfaces.Queries
{
    public interface IPlacePhotoQueries
    {
        Task<IReadOnlyList<PlacePhoto>> GetAllAsync(CancellationToken cancellationToken);
    }
}
