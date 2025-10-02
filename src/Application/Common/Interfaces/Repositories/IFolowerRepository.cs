using Domain.Cities;
using Domain.Folowers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface IFolowerRepository
    {
        Task<Folower> AddAsync(Folower entity, CancellationToken cancellationToken);
    }
}
