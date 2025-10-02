using Domain.Cities;
using Domain.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface ICountryRepository
    {
        Task<Country> AddAsync(Country entity, CancellationToken cancellationToken);
    }
}
