using Domain.Feedbacks;

namespace Api.Dtos;

public record FeedbackDto(Guid Id, string Comment, int Rating, DateTime Date, DateTime? UpdatedAt, Guid UserId, Guid PlaceId)
{
    public static FeedbackDto FromDomainModel(Feedback feedback)
        => new(feedback.Id.Value, feedback.Comment, feedback.Rating, feedback.Date, feedback.UpdatedAt, feedback.UserId.Value, feedback.PlaceId.Value);
}

public record CreateFeedbackDto(string Comment, int Rating, Guid UserId, Guid PlaceId);

public record UpdateFeedbackDto(Guid Id, string Comment, int Rating);