using Application.Common.Interfaces.Repositories;
using Application.Entities.Followers.Exceptions;
using Domain.Followers;
using Domain.Users;
using LanguageExt;
using MediatR;
using static LanguageExt.Prelude;
using Unit = LanguageExt.Unit;

namespace Application.Entities.Followers.Commands;

public record CreateFollowerCommand : IRequest<Either<FollowerException, Follower>>
{
    public required Guid FollowerUserId { get; init; }
    public required Guid FollowedUserId { get; init; }
}

public class CreateFollowerCommandHandler(
    IFollowerRepository followerRepository,
    IUserRepository userRepository)
    : IRequestHandler<CreateFollowerCommand, Either<FollowerException, Follower>>
{
    public async Task<Either<FollowerException, Follower>> Handle(
        CreateFollowerCommand request,
        CancellationToken cancellationToken)
    {
        var followerId = new UserId(request.FollowerUserId);
        var followedId = new UserId(request.FollowedUserId);

        return await ValidateNotSelf(followerId, followedId)
            .BindAsync(_ => EnsureUserExists(followerId, isFollower: true, cancellationToken))
            .BindAsync(_ => EnsureUserExists(followedId, isFollower: false, cancellationToken))
            .BindAsync(_ => EnsureRelationNotExists(followerId, followedId, cancellationToken))
            .BindAsync(_ => CreateEntity(followerId, followedId, cancellationToken));
    }

    // followerId != followedId
    private static Either<FollowerException, Unit> ValidateNotSelf(UserId followerId, UserId followedId)
        => followerId == followedId
            ? new CannotFollowYourselfException(followerId)
            : Right<FollowerException, Unit>(Unit.Default);

    // користувач існує (для обох: follower та followed)
    private async Task<Either<FollowerException, Unit>> EnsureUserExists(
        UserId userId,
        bool isFollower,
        CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);

        return user.Match<Either<FollowerException, Unit>>(
            _ => Right<FollowerException, Unit>(Unit.Default),
            () => isFollower
                ? new FollowerUserNotFoundException(userId)
                : new FollowedUserNotFoundException(userId));
    }

    // зв’язок ще не існує
    private async Task<Either<FollowerException, Unit>> EnsureRelationNotExists(
        UserId followerId,
        UserId followedId,
        CancellationToken ct)
    {
        var existing = await followerRepository.GetByIdsAsync(followerId, followedId, ct);

        // якщо репозиторій повертає Option<Follower>
        return existing.Match<Either<FollowerException, Unit>>(
            _ => new FollowerAlreadyExistException(followerId, followedId),
            () => Right<FollowerException, Unit>(Unit.Default));
    }

    // створення
    private async Task<Either<FollowerException, Follower>> CreateEntity(
        UserId followerId,
        UserId followedId,
        CancellationToken ct)
    {
        try
        {
            var entity = Follower.New(followerId, followedId);
            var created = await followerRepository.AddAsync(entity, ct);
            return created;
        }
        catch (Exception ex)
        {
            return new UnhandledFollowerException(followerId, followedId, ex);
        }
    }
}
