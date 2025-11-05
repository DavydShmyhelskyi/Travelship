using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Repositories;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class FeedbackControllerService(IFeedbackRepository feedbackRepository) : IFeedbackControllerService
    {
        public async Task<Option<FeedbackDto>> Get(Guid feedbackId, CancellationToken cancellationToken)
        {
            var entity = await feedbackRepository.GetByIdAsync(new Domain.Feedbacks.FeedbackId(feedbackId), cancellationToken);
            return entity.Match(
                r => FeedbackDto.FromDomainModel(r),
                () => Option<FeedbackDto>.None);
        }
    }
}
