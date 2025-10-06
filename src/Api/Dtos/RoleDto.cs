using Domain.Roles;

namespace Api.Dtos;

public record RoleDto(Guid Id, string Title)
{
    public static RoleDto FromDomainModel(Role role)
        => new(role.Id, role.Title);
}

public record CreateRoleDto(string Title);
