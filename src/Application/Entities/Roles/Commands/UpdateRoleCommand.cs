using Application.Common.Interfaces.Repositories;
using Application.Entities.Roles.Exceptions;
using Domain.Roles;
using LanguageExt;
using MediatR;
using System.IO;
using Unit = LanguageExt.Unit;

namespace Application.Entities.Roles.Commands;

public record UpdateRoleCommand : IRequest<Either<RoleException, Role>>
{
    public required Guid RoleId { get; init; }
    public required string Title { get; init; }
}

public class UpdateRoleCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<UpdateRoleCommand, Either<RoleException, Role>>
{
    public async Task<Either<RoleException, Role>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var roleId = new RoleId(request.RoleId);

        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);

        return await role.MatchAsync(
            r => CheckDuplicates(r.Id, request.Title, cancellationToken)
                .BindAsync(_ => UpdateEntity(request, r, cancellationToken)),
            () => new RoleNotFoundException(roleId));
    }

    private async Task<Either<RoleException, Role>> UpdateEntity(
        UpdateRoleCommand request,
        Role role,
        CancellationToken cancellationToken)
    {
        try
        {
            role.ChangeTitle(request.Title);
            return await roleRepository.UpdateAsync(role, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledRoleException(role.Id, ex);
        }
    }

    private async Task<Either<RoleException, Unit>> CheckDuplicates(
        RoleId currentRoleId,
        string title,
        CancellationToken cancellationToken)
    {
        var existing = await roleRepository.GetByTitleAsync(title, cancellationToken);

        return existing.Match<Either<RoleException, Unit>>(
            r => r.Id.Equals(currentRoleId) ? Unit.Default : new RoleAlreadyExistException(r.Id),
            () => Unit.Default);
    }
}
