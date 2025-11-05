using Api.Dtos;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IPlacePhotoControllerService
    {
        Task<Option<PlacePhotoDto>> Get(Guid placePhotoId, CancellationToken cancellationToken);
    }
}
