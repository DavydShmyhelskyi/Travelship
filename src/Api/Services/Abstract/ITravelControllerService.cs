using Api.Dtos;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface ITravelControllerService
    {
        Task<Option<TravelDto>> Get(Guid travelId, CancellationToken cancellationToken);
    }
}
