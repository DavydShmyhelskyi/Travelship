using Api.Dtos;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IFeedbackControllerService
    {
        Task<Option<FeedbackDto>> Get(Guid feedbackId, CancellationToken cancellationToken);
    }
}
