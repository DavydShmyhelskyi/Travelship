using Api.Dtos;
using Api.Modules.Errors;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.Followers.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("followers")]
[ApiController]
public class FollowersController(
    IFollowerQueries followerQueries,
    IFollowerControllerService controllerService,
    ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FollowerDto>>> GetFollowers(CancellationToken cancellationToken)
    {
        var followers = await followerQueries.GetAllAsync(cancellationToken);
        return followers.Select(FollowerDto.FromDomainModel).ToList();
    }
    [HttpGet("{followerId:guid}/{followedId:guid}")]
    public async Task<ActionResult<FollowerDto>> Get(
        [FromRoute] Guid followerId,
        [FromRoute] Guid followedId,
        CancellationToken cancellationToken)
    {
        var entity = await controllerService.Get(followerId, followedId, cancellationToken);

        return entity.Match<ActionResult<FollowerDto>>(
            f => f,
            () => NotFound()
        );
    }

    [HttpPost]
    public async Task<ActionResult<FollowerDto>> CreateFollower(
        [FromBody] CreateFollowerDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateFollowerCommand
        {
            FollowerUserId = request.FollowerUserId,
            FollowedUserId = request.FollowedUserId
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<FollowerDto>>(
            f => FollowerDto.FromDomainModel(f),
            e => e.ToObjectResult());
    }

    [HttpDelete("{followerId:guid}/{followedId:guid}")]
    public async Task<ActionResult> DeleteFollower(
        [FromRoute] Guid followerId,
        [FromRoute] Guid followedId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteFollowerCommand
        {
            FollowerUserId = followerId,
            FollowedUserId = followedId
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult>(
            _ => NoContent(),
            e => e.ToObjectResult());
    }
}
