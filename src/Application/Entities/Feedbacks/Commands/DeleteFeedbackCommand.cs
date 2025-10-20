using Application.Common.Interfaces.Repositories;
using Application.Entities.Feedbacks.Exceptions;
using Domain.Feedbacks;
using LanguageExt;
using MediatR;

namespace Application.Entities.Feedbacks.Commands;

public record DeleteFeedbackCommand : IRequest<Either<FeedbackException, Feedback>>
{
    public required Guid FeedbackId { get; init; }
}

public class DeleteFeedbackCommandHandler(IFeedbackRepository feedbackRepository)
    : IRequestHandler<DeleteFeedbackCommand, Either<FeedbackException, Feedback>>
{
    public async Task<Either<FeedbackException, Feedback>> Handle(
        DeleteFeedbackCommand request,
        CancellationToken cancellationToken)
    {
        var feedbackId = new FeedbackId(request.FeedbackId);
        var feedback = await feedbackRepository.GetByIdAsync(feedbackId, cancellationToken);

        return await feedback.MatchAsync(
            f => DeleteEntity(f, cancellationToken),
            () => new FeedbackNotFoundException(feedbackId));
    }

    private async Task<Either<FeedbackException, Feedback>> DeleteEntity(
        Feedback feedback,
        CancellationToken cancellationToken)
    {
        try
        {
            return await feedbackRepository.DeleteAsync(feedback, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledFeedbackException(feedback.Id, ex);
        }
    }
}
