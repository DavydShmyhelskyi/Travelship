using Domain.Roles;

namespace Application.Entities.Roles.Exceptions;

public abstract class RoleException(RoleId roleId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public RoleId RoleId { get; } = roleId;
}

public class RoleAlreadyExistException(RoleId roleId) : RoleException(roleId, $"Country already exists under id {roleId}");

public class RoleNotFoundException(RoleId roleId) : RoleException(roleId, $"Country not found under id {roleId}");

public class UnhandledRoleException(RoleId roleId, Exception? innerException = null)
    : RoleException(roleId, "Unexpected error occurred", innerException);