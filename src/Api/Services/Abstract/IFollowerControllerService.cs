using Api.Dtos;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IFollowerControllerService
    {
        Task<Option<FollowerDto>> Get(Guid followerId, Guid followedId, CancellationToken cancellationToken);
    }
}
