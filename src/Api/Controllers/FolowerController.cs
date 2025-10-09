using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Entities.Folowers.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("followers")]
[ApiController]
public class FollowersController(
    IFolowerQueries followerQueries,
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
            FollowerId = request.FollowerId,
            FollowedId = request.FollowedId
        };

        var newFollower = await sender.Send(input, cancellationToken);
        return FollowerDto.FromDomainModel(newFollower);
    }
}
