using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Entities.Followers.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers;

[Route("followers")]
[ApiController]
public class FollowersController(
    IFollowerQueries followerQueries,
    ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FollowerDto>>> GetFollowers(CancellationToken cancellationToken)
    {
        var followers = await followerQueries.GetAllAsync(cancellationToken);
        return followers.Select(FollowerDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<FollowerDto>> CreateFollower(
        [FromBody] CreateFollowerDto request,
        CancellationToken cancellationToken)
    {

        var input = new CreateFollowerCommand
        {
            FollowerUserId = request.FollowerId,
            FollowedUserId = request.FollowedId
        };

        var newFollower = await sender.Send(input, cancellationToken);
        return FollowerDto.FromDomainModel(newFollower);
    }
}
