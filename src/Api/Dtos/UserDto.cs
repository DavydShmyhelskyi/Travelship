using Domain.Users;

namespace Api.Dtos;

public record UserDto(Guid Id, string NickName, byte[]? Avatar, string Email, DateTime CreatedAt, Guid RoleId, Guid? CityId)
{
    public static UserDto FromDomainModel(User user)
        => new(user.Id.Value, user.NickName, user.Avatar, user.Email, user.CreatedAt, user.RoleId.Value, user.CityId?.Value);
}

public record CreateUserDto(string NickName, byte[]? Avatar, string Email, string Password, Guid RoleId, Guid? CityId);
public record UpdateUserDto(Guid Id, string NickName, byte[]? Avatar, string Email, Guid RoleId, Guid? CityId);