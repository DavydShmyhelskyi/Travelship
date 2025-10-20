using Domain.Users;

namespace Application.Entities.Followers.Exceptions;

public abstract class FollowerException(UserId followerUserId, UserId followedUserId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public UserId FollowerUserId { get; } = followerUserId;
    public UserId FollowedUserId { get; } = followedUserId;
}

public class FollowerAlreadyExistException(UserId followerUserId, UserId followedUserId)
    : FollowerException(followerUserId, followedUserId, $"Follower relationship already exists between {followerUserId} and {followedUserId}");

public class FollowerNotFoundException(UserId followerUserId, UserId followedUserId)
    : FollowerException(followerUserId, followedUserId, $"Follower relationship not found between {followerUserId} and {followedUserId}");

public class InvalidFollowerOperationException(string message)
    : Exception(message);

public class UnhandledFollowerException(UserId followerUserId, UserId followedUserId, Exception? innerException = null)
    : FollowerException(followerUserId, followedUserId, "Unexpected error occurred", innerException);

public class CannotFollowYourselfException(UserId userId)
    : FollowerException(userId, userId, "A user cannot follow themselves.");