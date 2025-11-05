using Api.Dtos;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IPlaceControllerService
    {
        Task<Option<PlaceDto>> Get(Guid placeId, CancellationToken cancellationToken);
    }
}
