using Domain.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface ICityQueries
    {
        Task<IReadOnlyList<City>> GetAllAsync(CancellationToken cancellationToken);
    }
}
