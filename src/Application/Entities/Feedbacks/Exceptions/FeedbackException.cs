using Domain.Feedbacks;

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
