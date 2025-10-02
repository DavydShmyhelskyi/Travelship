using Domain.PlacePhotos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface IPlacePhotoQueries
    {
        Task<IReadOnlyList<PlacePhoto>> GetAllAsync(CancellationToken cancellationToken);
    }
}
