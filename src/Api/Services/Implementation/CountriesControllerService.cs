using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Domain.Countries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class CountriesControllerService(ICountryQueries countryQueries) : ICountriesControllerService
    {
        public async Task<Option<CountryDto>> Get(Guid id, CancellationToken cancellationToken)
        {
                       var entity = await countryQueries.GetByIdAsync(new CountryId(id), cancellationToken);
            return entity.Match(
                r => CountryDto.FromDomainModel(r),
                () => Option<CountryDto>.None);
        }
    }
}
