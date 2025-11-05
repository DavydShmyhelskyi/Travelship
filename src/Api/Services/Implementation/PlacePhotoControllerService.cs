using Api.Dtos;
using Api.Services.Abstract;
using Application.Common.Interfaces.Repositories;
using Domain.PlacePhotos;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class PlacePhotoControllerService(IPlacePhotoRepository placePhotoRepository) : IPlacePhotoControllerService
    {
        public async Task<Option<PlacePhotoDto>> Get(Guid placePhotoId, CancellationToken cancellationToken)
        {
            var entity = await placePhotoRepository.GetByIdAsync(new Domain.PlacePhotos.PlacePhotoId(placePhotoId), cancellationToken);
            return entity.Match(
                r => PlacePhotoDto.FromDomainModel(r),
                () => Option<PlacePhotoDto>.None);
        }
    }
}
