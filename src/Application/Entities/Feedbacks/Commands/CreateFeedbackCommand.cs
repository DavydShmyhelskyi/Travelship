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

public class CreateFeedbackCommandHandler(
    IFeedbackRepository feedbackRepository,
    IUserRepository userRepository,
    IPlaceRepository placeRepository)
    : IRequestHandler<CreateFeedbackCommand, Either<FeedbackException, Feedback>>
{
    public async Task<Either<FeedbackException, Feedback>> Handle(
        CreateFeedbackCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var placeId = new PlaceId(request.PlaceId);

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user.IsNone)
            return new FeedbackUserNotFoundException(userId);

        var place = await placeRepository.GetByIdAsync(placeId, cancellationToken);
        if (place.IsNone)
            return new FeedbackPlaceNotFoundException(placeId);

        try
        {
            var feedback = Feedback.New(request.Comment, request.Rating, userId, placeId);
            var created = await feedbackRepository.AddAsync(feedback, cancellationToken);
            return created;
        }
        catch (Exception ex)
        {
            return new UnhandledFeedbackException(FeedbackId.Empty(), ex);
        }
    }
}
