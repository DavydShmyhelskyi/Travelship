using Domain.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface ICountryQueries
    {
        Task<IReadOnlyList<Country>> GetAllAsync(CancellationToken cancellationToken);
    }
}
