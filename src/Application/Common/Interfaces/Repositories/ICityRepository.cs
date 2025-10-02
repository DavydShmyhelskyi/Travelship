using Domain.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface ICityRepository
    {
        Task<City> AddAsync(City entity, CancellationToken cancellationToken);
    }
}
