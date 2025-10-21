using Domain.Followers;

namespace Api.Dtos;

public record FollowerDto(Guid FollowerId, Guid FollowedId, DateTime Date)
{
    public static FollowerDto FromDomainModel(Follower follower)
    => new(
        follower.FollowerUserId.Value,
        follower.FollowedUserId.Value,
        follower.Date
    );

}

public record CreateFollowerDto(Guid FollowerId, Guid FollowedId);
