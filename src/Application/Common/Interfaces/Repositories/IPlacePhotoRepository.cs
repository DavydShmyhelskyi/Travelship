using Domain.Cities;
using Domain.PlacePhotos;
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
    }
}
