using Domain.Folowers;

namespace Api.Dtos;

public record FollowerDto(Guid FollowerId, Guid FollowedId, DateTime Date)
{
    public static FollowerDto FromDomainModel(Folower follower)
        => new(follower.FollowerId, follower.FollowedId, follower.Date);
}

public record CreateFollowerDto(Guid FollowerId, Guid FollowedId);
