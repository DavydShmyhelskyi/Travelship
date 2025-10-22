using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Entities.Places.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("places")]
[ApiController]
public class PlacesController(
    IPlaceQueries placeQueries,
    ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PlaceDto>>> GetPlaces(CancellationToken cancellationToken)
    {
        var places = await placeQueries.GetAllAsync(cancellationToken);
        return places.Select(PlaceDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<PlaceDto>> CreatePlace(
        [FromBody] CreatePlaceDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreatePlaceCommand
        {
            Title = request.Title,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<PlaceDto>>(
            p => PlaceDto.FromDomainModel(p),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<PlaceDto>> UpdatePlace(
        [FromBody] UpdatePlaceDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePlaceCommand
        {
            PlaceId = request.Id,
            Title = request.Title,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<PlaceDto>>(
            p => PlaceDto.FromDomainModel(p),
            e => e.ToObjectResult());
    }

    [HttpDelete("{placeId:guid}")]
    public async Task<ActionResult<PlaceDto>> DeletePlace(Guid placeId, CancellationToken cancellationToken)
    {
        var command = new DeletePlaceCommand { PlaceId = placeId };
        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<PlaceDto>>(
            p => PlaceDto.FromDomainModel(p),
            e => e.ToObjectResult());
    }
}
