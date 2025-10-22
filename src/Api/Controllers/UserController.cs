using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Entities.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("users")]
[ApiController]
public class UsersController(
    IUserQueries userQueries,
    ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await userQueries.GetAllAsync(cancellationToken);
        return users.Select(UserDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(
        [FromBody] CreateUserDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand
        {
            NickName = request.NickName,
            Avatar = request.Avatar,
            Email = request.Email,
            Password = request.Password,
            RoleId = request.RoleId,
            CityId = request.CityId
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<UserDto>>(
            u => UserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<UserDto>> UpdateUser(
        [FromBody] UpdateUserDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand
        {
            Id = request.Id,
            NickName = request.NickName,
            Avatar = request.Avatar,
            Email = request.Email,
            RoleId = request.RoleId,
            CityId = request.CityId
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<UserDto>>(
            u => UserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }

    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult<UserDto>> DeleteUser(Guid userId, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand { UserId = userId };
        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<UserDto>>(
            u => UserDto.FromDomainModel(u),
            e => e.ToObjectResult());
    }
}
