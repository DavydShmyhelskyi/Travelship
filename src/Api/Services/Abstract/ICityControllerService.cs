using Api.Dtos;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface ICitiesControllerService
    {
        Task<Option<CityDto>> Get(Guid cityId, CancellationToken cancellationToken);
    }
}
