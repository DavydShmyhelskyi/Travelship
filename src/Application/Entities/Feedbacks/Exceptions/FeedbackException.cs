using Domain.Feedbacks;
using Domain.Places;
using Domain.Users;

namespace Application.Entities.Feedbacks.Exceptions;

public abstract class FeedbackException(FeedbackId feedbackId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public FeedbackId FeedbackId { get; } = feedbackId;
}

public class FeedbackNotFoundException(FeedbackId feedbackId)
    : FeedbackException(feedbackId, $"Feedback not found under id {feedbackId}");

public class FeedbackAlreadyExistException(FeedbackId feedbackId)
    : FeedbackException(feedbackId, $"Feedback already exists under id {feedbackId}");

public class UnhandledFeedbackException(FeedbackId feedbackId, Exception? innerException = null)
    : FeedbackException(feedbackId, "Unexpected error occurred", innerException);
public class FeedbackUserNotFoundException(UserId userId)
    : FeedbackException(FeedbackId.Empty(), $"User not found under id {userId}");

public class FeedbackPlaceNotFoundException(PlaceId placeId)
    : FeedbackException(FeedbackId.Empty(), $"Place not found under id {placeId}");