using Application.Common.Interfaces.Repositories;
using Application.Entities.Roles.Exceptions;
using Domain.Roles;
using LanguageExt;
using MediatR;

namespace Application.Entities.Roles.Commands;

public record DeleteRoleCommand : IRequest<Either<RoleException, Role>>
{
    public required Guid RoleId { get; init; }
}

public class DeleteRoleCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<DeleteRoleCommand, Either<RoleException, Role>>
{
    public async Task<Either<RoleException, Role>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var roleId = new RoleId(request.RoleId);
        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);

        return await role.MatchAsync(
            r => DeleteEntity(r, cancellationToken),
            () => new RoleNotFoundException(roleId));
    }

    private async Task<Either<RoleException, Role>> DeleteEntity(Role role, CancellationToken cancellationToken)
    {
        try
        {
            return await roleRepository.DeleteAsync(role, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledRoleException(role.Id, ex);
        }
    }
}
