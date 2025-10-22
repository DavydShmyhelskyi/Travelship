using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Entities.PlacePhotos.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("place-photos")]
[ApiController]
public class PlacePhotosController(
    IPlacePhotoQueries placePhotoQueries,
    ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PlacePhotoDto>>> GetPlacePhotos(CancellationToken cancellationToken)
    {
        var photos = await placePhotoQueries.GetAllAsync(cancellationToken);
        return photos.Select(PlacePhotoDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<PlacePhotoDto>> CreatePlacePhoto(
        [FromBody] CreatePlacePhotoDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreatePlacePhotoCommand
        {
            Photo = request.Photo,
            Description = request.Description,
            PlaceId = request.PlaceId
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<PlacePhotoDto>>(
            p => PlacePhotoDto.FromDomainModel(p),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<PlacePhotoDto>> UpdatePlacePhoto(
        [FromBody] UpdatePlacePhotoDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePlacePhotoCommand
        {
            PlacePhotoId = request.Id,
            Photo = request.Photo,
            Description = request.Description,
            IsShown = request.IsShown
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<PlacePhotoDto>>(
            p => PlacePhotoDto.FromDomainModel(p),
            e => e.ToObjectResult());
    }

    [HttpDelete("{photoId:guid}")]
    public async Task<ActionResult<PlacePhotoDto>> DeletePlacePhoto(Guid photoId, CancellationToken cancellationToken)
    {
        var command = new DeletePlacePhotoCommand { PlacePhotoId = photoId };
        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<PlacePhotoDto>>(
            p => PlacePhotoDto.FromDomainModel(p),
            e => e.ToObjectResult());
    }
}
