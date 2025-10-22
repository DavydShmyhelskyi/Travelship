using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Entities.Roles.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("roles")]
[ApiController]
public class RolesController(
    IRoleQueries roleQueries,
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
        var command = new CreateRoleCommand { Title = request.Title };
        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<RoleDto>> UpdateRole(
        [FromBody] UpdateRoleDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand
        {
            RoleId = request.Id,
            Title = request.Title
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }

    [HttpDelete("{roleId:guid}")]
    public async Task<ActionResult<RoleDto>> DeleteRole(Guid roleId, CancellationToken cancellationToken)
    {
        var command = new DeleteRoleCommand { RoleId = roleId };
        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<RoleDto>>(
            r => RoleDto.FromDomainModel(r),
            e => e.ToObjectResult());
    }
}
