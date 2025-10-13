using Domain.Feedbacks;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface IFeedbackRepository
    {
        Task<Feedback> AddAsync(Feedback entity, CancellationToken cancellationToken);
        Task<Feedback> UpdateAsync(Feedback entity, CancellationToken cancellationToken);
        Task<Feedback> DeleteAsync(Feedback entity, CancellationToken cancellationToken);
        Task<Option<Feedback>> GetByRatingAsync(int rating, CancellationToken cancellationToken);
        Task<Option<Feedback>> GetByIdAsync(FeedbackId id, CancellationToken cancellationToken);
    }
}
