using Application.Common.Interfaces.Repositories;
using Application.Entities.Feedbacks.Exceptions;
using Domain.Feedbacks;
using Domain.Places;
using Domain.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.Feedbacks.Commands;

public record CreateFeedbackCommand : IRequest<Either<FeedbackException, Feedback>>
{
    public required string Comment { get; init; }
    public required int Rating { get; init; }
    public required Guid UserId { get; init; }
    public required Guid PlaceId { get; init; }
}

public class CreateFeedbackCommandHandler(IFeedbackRepository feedbackRepository)
    : IRequestHandler<CreateFeedbackCommand, Either<FeedbackException, Feedback>>
{
    public async Task<Either<FeedbackException, Feedback>> Handle(
        CreateFeedbackCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var feedback = Feedback.New(
                request.Comment,
                request.Rating,
                new UserId(request.UserId),
                new PlaceId(request.PlaceId));

            var created = await feedbackRepository.AddAsync(feedback, cancellationToken);
            return created;
        }
        catch (Exception ex)
        {
            return new UnhandledFeedbackException(FeedbackId.Empty(), ex);
        }
    }
}
