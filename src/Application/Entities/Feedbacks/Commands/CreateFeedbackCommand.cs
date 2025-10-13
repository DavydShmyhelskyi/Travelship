using Application.Common.Interfaces.Repositories;
using Domain.Feedbacks;
using Domain.Places;
using Domain.Users;
using MediatR;

namespace Application.Entities.Feedbacks.Commands;

public class CreateFeedbackCommand : IRequest<Feedback>
{
    public required string Comment { get; set; }
    public required int Rating { get; set; }          
    public required UserId UserId { get; set; }         
    public required PlaceId PlaceId { get; set; }        
}
public class CreateFeedbackCommandHandler(IFeedbackRepository feedbackRepository)
    : IRequestHandler<CreateFeedbackCommand, Feedback>
{
    public async Task<Feedback> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = Feedback.New(
            request.Comment,
            request.Rating,
            request.UserId,
            request.PlaceId
        );

        return await feedbackRepository.AddAsync(feedback, cancellationToken);
    }
}