using Domain.Feedbacks;

namespace Api.Dtos;

public record FeedbackDto(Guid Id, string Comment, int Rating, DateTime Date, DateTime? UpdatedAt, Guid UserId, Guid PlaceId)
{
    public static FeedbackDto FromDomainModel(Feedback feedback)
        => new(feedback.Id, feedback.Comment, feedback.Rating, feedback.Date, feedback.UpdatedAt, feedback.UserId, feedback.PlaceId);
}

public record CreateFeedbackDto(string Comment, int Rating, Guid UserId, Guid PlaceId);
