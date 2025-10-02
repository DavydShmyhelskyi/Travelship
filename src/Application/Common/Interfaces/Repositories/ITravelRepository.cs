using Domain.Cities;
using Domain.Travels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface ITravelRepository
    {
        Task<Travel> AddAsync(Travel entity, CancellationToken cancellationToken);
    }
}
