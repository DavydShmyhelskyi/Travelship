using Application.Common.Interfaces.Repositories;
using Application.Entities.Users.Exceptions;
using Domain.Cities;
using Domain.Roles;
using Domain.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.Users.Commands;

public record CreateUserCommand : IRequest<Either<UserException, User>>
{
    public required string NickName { get; init; }
    public byte[]? Avatar { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required Guid RoleId { get; init; }
    public Guid? CityId { get; init; }
}

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    ICityRepository cityRepository)
    : IRequestHandler<CreateUserCommand, Either<UserException, User>>
{
    public async Task<Either<UserException, User>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var roleId = new RoleId(request.RoleId);
        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role.IsNone)
            return new RoleForUserNotFoundException(UserId.Empty(), roleId);

        if (request.CityId.HasValue)
        {
            var cityId = new CityId(request.CityId.Value);
            var city = await cityRepository.GetByIdAsync(cityId, cancellationToken);
            if (city.IsNone)
                return new CityForUserNotFoundException(UserId.Empty(), cityId);
        }

        var existingByEmail = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        return await existingByEmail.MatchAsync(
            u => new UserEmailAlreadyExistsException(u.Id, request.Email),
            async () =>
            {
                var existingByNick = await userRepository.GetByNickNameAsync(request.NickName, cancellationToken);
                return await existingByNick.MatchAsync(
                    u => new UserNickNameAlreadyExistsException(u.Id, request.NickName),
                    () => CreateEntity(request, cancellationToken));
            });
    }

    private async Task<Either<UserException, User>> CreateEntity(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = User.New(
                request.NickName,
                request.Avatar,
                request.Email,
                request.Password,
                new RoleId(request.RoleId),
                request.CityId.HasValue ? new CityId(request.CityId.Value) : null);

            var created = await userRepository.AddAsync(user, cancellationToken);
            return created;
        }
        catch (Exception ex)
        {
            return new UnhandledUserException(UserId.Empty(), ex);
        }
    }
}
