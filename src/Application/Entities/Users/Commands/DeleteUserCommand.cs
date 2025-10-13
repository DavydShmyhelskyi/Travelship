using Application.Common.Interfaces.Repositories;
using Application.Entities.Users.Exceptions;
using Domain.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.Users.Commands;

public record DeleteUserCommand : IRequest<Either<UserException, User>>
{
    public required Guid UserId { get; init; }
}

public class DeleteUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<DeleteUserCommand, Either<UserException, User>>
{
    public async Task<Either<UserException, User>> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var existingUser = await userRepository.GetByIdAsync(userId, cancellationToken);

        return await existingUser.MatchAsync(
            u => DeleteEntity(u, cancellationToken),
            () => new UserNotFoundException(userId));
    }

    private async Task<Either<UserException, User>> DeleteEntity(
        User user,
        CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await userRepository.DeleteAsync(user, cancellationToken);
            return deleted;
        }
        catch (Exception ex)
        {
            return new UnhandledUserException(user.Id, ex);
        }
    }
}
