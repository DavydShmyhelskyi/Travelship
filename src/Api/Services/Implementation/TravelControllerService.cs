using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Repositories;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class TravelControllerService(ITravelRepository travelRepository) : ITravelControllerService
    {
        public async Task<Option<TravelDto>> Get(Guid travelId, CancellationToken cancellationToken)
        {
            var entity = await travelRepository.GetByIdAsync(new Domain.Travels.TravelId(travelId), cancellationToken);
            return entity.Match(
                t => TravelDto.FromDomainModel(t),
                () => Option<TravelDto>.None);
        }
    }
}
