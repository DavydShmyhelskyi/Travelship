using Application.Common.Interfaces.Repositories;
using Domain.Cities;
using Domain.Roles;
using Domain.Users;
using MediatR;

namespace Application.Entities.Users.Commands;

public class CreateUserCommand : IRequest<User>
{
    public required string NickName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }

    public required RoleId RoleId { get; set; }   
    public CityId? CityId { get; set; }
}

public class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, User>
{
    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.New(
            request.NickName,
            request.Email,
            request.Password,
            request.RoleId,
            request.CityId
        );

        return await userRepository.AddAsync(user, cancellationToken);
    }
}
