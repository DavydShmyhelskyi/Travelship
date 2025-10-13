using Domain.Feedbacks;

namespace Application.Common.Interfaces.Queries
{
    public interface IFeedbackQueries
    {
        Task<IReadOnlyList<Feedback>> GetAllAsync(CancellationToken cancellationToken);
    }
}
