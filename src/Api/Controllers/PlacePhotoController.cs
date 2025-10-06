using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Entities.PlacePhotos.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("placephotos")]
[ApiController]
public class PlacePhotosController(
    IPlacePhotoQueries placePhotoQueries,
    IValidator<CreatePlacePhotoDto> createPlacePhotoDtoValidator,
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
        var validationResult = createPlacePhotoDtoValidator.Validate(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var input = new CreatePlacePhotoCommand
        {
            Photo = request.Photo,
            Description = request.Description,
            PlaceId = request.PlaceId
        };

        var newPhoto = await sender.Send(input, cancellationToken);
        return PlacePhotoDto.FromDomainModel(newPhoto);
    }
}
