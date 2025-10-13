using Application.Common.Interfaces.Repositories;
using Application.Entities.Roles.Exceptions;
using Domain.Roles;
using LanguageExt;
using MediatR;

namespace Application.Entities.Roles.Commands;

public record CreateRoleCommand : IRequest<Either<RoleException, Role>>
{
    public required string Title { get; init; }
}

public class CreateRoleCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<CreateRoleCommand, Either<RoleException, Role>>
{
    public async Task<Either<RoleException, Role>> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var existingRole = await roleRepository.GetByTitleAsync(request.Title, cancellationToken);

        return await existingRole.MatchAsync(
            r => new RoleAlreadyExistException(r.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<RoleException, Role>> CreateEntity(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var role = Role.New(RoleId.New(), request.Title);
            var created = await roleRepository.AddAsync(role, cancellationToken);
            return created;
        }
        catch (Exception ex)
        {
            return new UnhandledRoleException(RoleId.Empty(), ex);
        }
    }
}
