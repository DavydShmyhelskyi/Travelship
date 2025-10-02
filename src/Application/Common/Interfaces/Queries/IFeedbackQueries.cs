using Domain.Feedbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface IFeedbackQueries
    {
        Task<IReadOnlyList<Feedback>> GetAllAsync(CancellationToken cancellationToken);
    }
}
