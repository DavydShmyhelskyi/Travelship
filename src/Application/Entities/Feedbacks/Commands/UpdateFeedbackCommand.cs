using Application.Common.Interfaces.Repositories;
using Application.Entities.Feedbacks.Exceptions;
using Domain.Feedbacks;
using LanguageExt;
using MediatR;

namespace Application.Entities.Feedbacks.Commands;

public record UpdateFeedbackCommand : IRequest<Either<FeedbackException, Feedback>>
{
    public required Guid FeedbackId { get; init; }
    public required string Comment { get; init; }
    public required int Rating { get; init; }
}

public class UpdateFeedbackCommandHandler(IFeedbackRepository feedbackRepository)
    : IRequestHandler<UpdateFeedbackCommand, Either<FeedbackException, Feedback>>
{
    public async Task<Either<FeedbackException, Feedback>> Handle(
        UpdateFeedbackCommand request,
        CancellationToken cancellationToken)
    {
        var feedbackId = new FeedbackId(request.FeedbackId);
        var feedback = await feedbackRepository.GetByIdAsync(feedbackId, cancellationToken);

        return await feedback.MatchAsync(
            f => UpdateEntity(request, f, cancellationToken),
            () => new FeedbackNotFoundException(feedbackId));
    }

    private async Task<Either<FeedbackException, Feedback>> UpdateEntity(
        UpdateFeedbackCommand request,
        Feedback feedback,
        CancellationToken cancellationToken)
    {
        try
        {
            feedback.Update(request.Comment, request.Rating);
            return await feedbackRepository.UpdateAsync(feedback, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledFeedbackException(feedback.Id, ex);
        }
    }
}
