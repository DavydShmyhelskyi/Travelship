using Domain.Cities;
using Domain.Feedbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface IFeedbackRepository
    {
        Task<Feedback> AddAsync(Feedback entity, CancellationToken cancellationToken);
    }
}
