using Application.Common.Interfaces.Repositories;
using Application.Entities.Users.Exceptions;
using Domain.Cities;
using Domain.Roles;
using Domain.Users;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Entities.Users.Commands;

public record UpdateUserCommand : IRequest<Either<UserException, User>>
{
    public required Guid Id { get; init; }
    public required string NickName { get; init; }
    public byte[]? Avatar { get; init; }
    public required string Email { get; init; }
    public required Guid RoleId { get; init; }
    public Guid? CityId { get; init; }
}

public class UpdateUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<UpdateUserCommand, Either<UserException, User>>
{
    public async Task<Either<UserException, User>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.Id);
        var existingUser = await userRepository.GetByIdAsync(userId, cancellationToken);

        return await existingUser.MatchAsync(
            u => CheckDuplicates(u.Id, request.Email, request.NickName, cancellationToken)
                .BindAsync(_ => UpdateEntity(request, u, cancellationToken)),
            () => new UserNotFoundException(userId));
    }

    private async Task<Either<UserException, Unit>> CheckDuplicates(
        UserId currentUserId,
        string email,
        string nickname,
        CancellationToken cancellationToken)
    {
        var existingByEmail = await userRepository.GetByEmailAsync(email, cancellationToken);
        var emailCheck = existingByEmail.Match<Either<UserException, Unit>>(
            u => u.Id.Equals(currentUserId)
                ? Unit.Default
                : new UserEmailAlreadyExistsException(u.Id, email),
            () => Unit.Default);

        if (emailCheck.IsLeft)
            return emailCheck;

        var existingByNick = await userRepository.GetByNickNameAsync(nickname, cancellationToken);
        return existingByNick.Match<Either<UserException, Unit>>(
            u => u.Id.Equals(currentUserId)
                ? Unit.Default
                : new UserNickNameAlreadyExistsException(u.Id, nickname),
            () => Unit.Default);
    }

    private async Task<Either<UserException, User>> UpdateEntity(
        UpdateUserCommand request,
        User user,
        CancellationToken cancellationToken)
    {
        try
        {
            user.Update(
                request.NickName,
                request.Avatar,
                request.Email,
                new RoleId(request.RoleId),
                request.CityId.HasValue ? new CityId(request.CityId.Value) : null);

            var updated = await userRepository.UpdateAsync(user, cancellationToken);
            return updated;
        }
        catch (Exception ex)
        {
            return new UnhandledUserException(user.Id, ex);
        }
    }
}
