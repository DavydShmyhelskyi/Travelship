using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Entities.Roles.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("roles")]
[ApiController]
public class RolesController(
    IRoleQueries roleQueries,
    IValidator<CreateRoleDto> createRoleDtoValidator,
    ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetRoles(CancellationToken cancellationToken)
    {
        var roles = await roleQueries.GetAllAsync(cancellationToken);
        return roles.Select(RoleDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole(
        [FromBody] CreateRoleDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = createRoleDtoValidator.Validate(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var input = new CreateRoleCommand
        {
            Title = request.Title
        };

        var newRole = await sender.Send(input, cancellationToken);
        return RoleDto.FromDomainModel(newRole);
    }
}
