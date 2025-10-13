using Domain.Users;

namespace Application.Entities.Users.Exceptions;

public abstract class UserException(UserId userId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public UserId UserId { get; } = userId;
}

public class UserAlreadyExistsException(UserId userId)
    : UserException(userId, $"User already exists under id {userId}");

public class UserNotFoundException(UserId userId)
    : UserException(userId, $"User not found under id {userId}");
public class UserEmailAlreadyExistsException(UserId userId, string email) 
    : UserException(userId,$"User with this email {email} already exists.");

public class UserNickNameAlreadyExistsException(UserId userId, string nickname)
    : UserException(userId, $"User with this nickname {nickname} already exists.");

public class UnhandledUserException(UserId userId, Exception? innerException = null)
    : UserException(userId, "Unexpected error occurred", innerException);

public class InvalidUserPasswordException(UserId userId)
    : UserException(userId, "Invalid current password.");
