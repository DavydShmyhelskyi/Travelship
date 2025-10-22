using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Entities.Feedbacks.Commands;
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
        var command = new CreateFeedbackCommand
        {
            Comment = request.Comment,
            Rating = request.Rating,
            UserId = request.UserId,
            PlaceId = request.PlaceId
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<FeedbackDto>>(
            f => FeedbackDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<FeedbackDto>> UpdateFeedback(
        [FromBody] UpdateFeedbackDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateFeedbackCommand
        {
            FeedbackId = request.Id,
            Comment = request.Comment,
            Rating = request.Rating
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<FeedbackDto>>(
            f => FeedbackDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }

    [HttpDelete("{feedbackId:guid}")]
    public async Task<ActionResult<FeedbackDto>> DeleteFeedback(Guid feedbackId, CancellationToken cancellationToken)
    {
        var command = new DeleteFeedbackCommand { FeedbackId = feedbackId };
        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<FeedbackDto>>(
            f => FeedbackDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }
}
