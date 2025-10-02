using Application.Common.Interfaces.Repositories;
using Domain.Roles;
using MediatR;

namespace Application.Entities.Roles.Commands;

public class CreateRoleCommand : IRequest<Role>
{
    public required string Title { get; set; }
}
public class CreateRoleCommandHandler(IRoleRepository roleRepository) : IRequestHandler<CreateRoleCommand, Role>
{
    public async Task<Role> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.AddAsync(Role.New(request.Title), cancellationToken);
        return role;
    }
}


