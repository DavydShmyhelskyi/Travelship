using Application.Common.Interfaces.Repositories;
using Application.Entities.Users.Exceptions;
using Domain.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.Users.Commands;

public record ChangeUserPasswordCommand : IRequest<Either<UserException, User>>
{
    public required Guid UserId { get; init; }
    public required string CurrentPassword { get; init; }
    public required string NewPassword { get; init; }
}

public class ChangeUserPasswordCommandHandler(IUserRepository userRepository)
    : IRequestHandler<ChangeUserPasswordCommand, Either<UserException, User>>
{
    public async Task<Either<UserException, User>> Handle(
        ChangeUserPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var existing = await userRepository.GetByIdAsync(userId, cancellationToken);

        return await existing.MatchAsync(
            u => VerifyAndChange(u, request, cancellationToken),
            () => new UserNotFoundException(userId));
    }

    private async Task<Either<UserException, User>> VerifyAndChange(
    User user,
    ChangeUserPasswordCommand request,
    CancellationToken cancellationToken)
    {
        if (!user.VerifyPassword(request.CurrentPassword))
            return new InvalidUserPasswordException(user.Id);

        try
        {
            user.ChangePassword(request.NewPassword);
            var updated = await userRepository.UpdateAsync(user, cancellationToken);
            return updated;
        }
        catch (Exception ex)
        {
            return new UnhandledUserException(user.Id, ex);
        }
    }

}
