using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Entities.Places.Commands;
using FluentValidation;
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

        var input = new CreatePlaceCommand
        {
            Title = request.Title,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        var newPlace = await sender.Send(input, cancellationToken);
        return PlaceDto.FromDomainModel(newPlace);
    }
}
