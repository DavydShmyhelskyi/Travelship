using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Entities.Users.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("users")]
[ApiController]
public class UsersController(
    IUserQueries userQueries,
    IValidator<CreateUserDto> createUserDtoValidator,
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
        var validationResult = createUserDtoValidator.Validate(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var input = new CreateUserCommand
        {
            NickName = request.NickName,
            Email = request.Email,
            Password = request.Password,
            RoleId = request.RoleId,
            CityId = request.CityId
        };

        var newUser = await sender.Send(input, cancellationToken);
        return UserDto.FromDomainModel(newUser);
    }
}
