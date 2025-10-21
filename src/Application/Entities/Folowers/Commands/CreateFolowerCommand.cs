using Application.Common.Interfaces.Repositories;
using Application.Entities.Followers.Exceptions;
using Domain.Followers;
using Domain.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.Followers.Commands;

public record CreateFollowerCommand : IRequest<Either<FollowerException, Follower>>
{
    public required Guid FollowerUserId { get; init; }
    public required Guid FollowedUserId { get; init; }
}

public class CreateFollowerCommandHandler(IFollowerRepository followerRepository)
    : IRequestHandler<CreateFollowerCommand, Either<FollowerException, Follower>>
{
    public async Task<Either<FollowerException, Follower>> Handle(
        CreateFollowerCommand request,
        CancellationToken cancellationToken)
    {
        var followerId = new UserId(request.FollowerUserId);
        var followedId = new UserId(request.FollowedUserId);

        if (followerId == followedId)
        {
            return new CannotFollowYourselfException(followerId);
        }

        var existing = await followerRepository.GetByIdsAsync(followerId, followedId, cancellationToken);

        if (existing.IsSome)
        {
            return new FollowerAlreadyExistException(followerId, followedId);
        }

        return await CreateEntity(followerId, followedId, cancellationToken);
    }

    private async Task<Either<FollowerException, Follower>> CreateEntity(
        UserId followerId,
        UserId followedId,
        CancellationToken cancellationToken)
    {
        try
        {
            var follower = Follower.New(followerId, followedId);
            var created = await followerRepository.AddAsync(follower, cancellationToken);
            return created;
        }
        catch (Exception ex)
        {
            return new UnhandledFollowerException(followerId, followedId, ex);
        }
    }
}
