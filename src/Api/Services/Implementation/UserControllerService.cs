using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Repositories;
using Domain.Users;
using Infrastructure.Persistence.Repositories;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class UserControllerService(IUserRepository userRepository) : IUserControllerService
    {
        public async Task<Option<UserDto>> Get(Guid cityId, CancellationToken cancellationToken)
        {
            var entity = await userRepository.GetByIdAsync(new UserId(cityId), cancellationToken);
            return entity.Match(
                u => UserDto.FromDomainModel(u),
                () => Option<UserDto>.None);
        }
    }
}
