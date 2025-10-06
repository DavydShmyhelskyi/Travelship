using Domain.Users;

namespace Api.Dtos;

public record UserDto(Guid Id, string NickName, string Email, DateTime CreatedAt, Guid RoleId, Guid? CityId)
{
    public static UserDto FromDomainModel(User user)
        => new(user.Id, user.NickName, user.Email, user.CreatedAt, user.RoleId, user.CityId);
}

public record CreateUserDto(string NickName, string Email, string Password, Guid RoleId, Guid? CityId);
