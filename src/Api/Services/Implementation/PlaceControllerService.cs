using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Repositories;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class PlaceControllerService(IPlaceRepository placeRepository) : IPlaceControllerService
    {
        public async Task<Option<PlaceDto>> Get(Guid placeId, CancellationToken cancellationToken)
        {
            var entity = await placeRepository.GetByIdAsync(new Domain.Places.PlaceId(placeId), cancellationToken);
            return entity.Match(
                p => PlaceDto.FromDomainModel(p),
                () => Option<PlaceDto>.None);
        }
    }
}
