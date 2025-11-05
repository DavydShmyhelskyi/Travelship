using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Repositories;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class FollowerControllerService(IFollowerRepository followerRepository) : IFollowerControllerService
    {
        public async Task<Option<FollowerDto>> Get(Guid followerId, Guid followedId, CancellationToken cancellationToken)
        {
            var entity = await followerRepository.GetByIdsAsync(new Domain.Users.UserId(followerId), new Domain.Users.UserId(followedId), cancellationToken);
            return entity.Match(
                r => FollowerDto.FromDomainModel(r),
                () => Option<FollowerDto>.None);
        }
    }
}
