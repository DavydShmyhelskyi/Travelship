using Domain.Travels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface ITravelQueries
    {
        Task<IReadOnlyList<Travel>> GetAllAsync(CancellationToken cancellationToken);
    }
}
