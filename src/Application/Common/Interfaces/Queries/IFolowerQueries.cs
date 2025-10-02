using Domain.Folowers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface IFolowerQueries
    {
        Task<IReadOnlyList<Folower>> GetAllAsync(CancellationToken cancellationToken);
    }
}
