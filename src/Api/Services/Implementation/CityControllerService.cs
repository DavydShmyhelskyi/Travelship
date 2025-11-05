using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class CitiesControllerService(ICityRepository cityRepository) : ICitiesControllerService
    {
        public async Task<Option<CityDto>> Get(Guid cityId, CancellationToken cancellationToken)
        {
            var entity = await cityRepository.GetByIdAsync(new Domain.Cities.CityId(cityId), cancellationToken);
            return entity.Match(
                r => CityDto.FromDomainModel(r),
                () => Option<CityDto>.None);
        }
    }
}
