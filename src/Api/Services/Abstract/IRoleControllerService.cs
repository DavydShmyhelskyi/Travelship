using Api.Dtos;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IRoleControllerService
    {
        Task<Option<RoleDto>> Get(Guid cityId, CancellationToken cancellationToken);
    }
}
