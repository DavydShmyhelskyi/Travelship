using Domain.Cities;
using Domain.Places;
using LanguageExt;
using LanguageExt.ClassInstances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface IPlaceRepository
    {
        Task<Place> AddAsync(Place entity, CancellationToken cancellationToken);
        Task<Place> UpdateAsync(Place entity, CancellationToken cancellationToken);
        Task<Place> DeleteAsync(Place entity, CancellationToken cancellationToken);
        Task<Option<Place>> GetByTitleAsync(string title, CancellationToken cancellationToken);
        Task<Option<Place>> GetByIdAsync(PlaceId id, CancellationToken cancellationToken);
    }
}
