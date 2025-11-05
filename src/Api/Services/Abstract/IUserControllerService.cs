using Api.Dtos;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IUserControllerService
    {
        Task<Option<UserDto>> Get(Guid userId, CancellationToken cancellationToken);
    }
}
