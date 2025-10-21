using Domain.Roles;

namespace Api.Dtos;

public record RoleDto(Guid Id, string Title)
{
    public static RoleDto FromDomainModel(Role role)
        => new(role.Id.Value, role.Title);
}

public record CreateRoleDto(string Title);
