using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Entities.Feedbacks.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("feedbacks")]
[ApiController]
public class FeedbacksController(
    IFeedbackQueries feedbackQueries,
    ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FeedbackDto>>> GetFeedbacks(CancellationToken cancellationToken)
    {
        var feedbacks = await feedbackQueries.GetAllAsync(cancellationToken);
        return feedbacks.Select(FeedbackDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<FeedbackDto>> CreateFeedback(
        [FromBody] CreateFeedbackDto request,
        CancellationToken cancellationToken)
    {

        var input = new CreateFeedbackCommand
        {
            Comment = request.Comment,
            Rating = request.Rating,
            UserId = request.UserId,
            PlaceId = request.PlaceId
        };

        var newFeedback = await sender.Send(input, cancellationToken);
        return FeedbackDto.FromDomainModel(newFeedback);
    }
}
