using Domain.Cities;
using Domain.Places;
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
    }
}
