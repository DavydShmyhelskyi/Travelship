using Domain.Cities;
using Domain.PlacePhotos;
using LanguageExt;
using LanguageExt.ClassInstances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
