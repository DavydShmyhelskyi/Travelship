using Application.Common.Interfaces.Repositories;
using Application.Entities.Followers.Exceptions;
using Domain.Followers;
using Domain.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.Followers.Commands;

public record DeleteFollowerCommand : IRequest<Either<FollowerException, Follower>>
{
    public required Guid FollowerUserId { get; init; }
    public required Guid FollowedUserId { get; init; }
}

public class DeleteFollowerCommandHandler(IFollowerRepository followerRepository)
    : IRequestHandler<DeleteFollowerCommand, Either<FollowerException, Follower>>
{
    public async Task<Either<FollowerException, Follower>> Handle(
    DeleteFollowerCommand request,
    CancellationToken cancellationToken)
    {
        var followerId = new UserId(request.FollowerUserId);
        var followedId = new UserId(request.FollowedUserId);

        var existing = await followerRepository.GetByIdsAsync(followerId, followedId, cancellationToken);

        if (existing.IsNone)
        {
            return new FollowerNotFoundException(followerId, followedId);
        }

        var follower = existing.Match(
            Some: f => f,
            None: () => throw new InvalidOperationException("Unexpected empty follower")
        );

        return await DeleteEntity(follower, cancellationToken);
    }


    private async Task<Either<FollowerException, Follower>> DeleteEntity(
        Follower follower,
        CancellationToken cancellationToken)
    {
        try
        {
            return await followerRepository.DeleteAsync(follower, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledFollowerException(follower.FollowerUserId, follower.FollowedUserId, ex);
        }
    }
}
