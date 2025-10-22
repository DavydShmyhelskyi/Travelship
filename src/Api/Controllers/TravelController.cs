using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Entities.Travels.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("travels")]
[ApiController]
public class TravelsController(
    ITravelQueries travelQueries,
    ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TravelDto>>> GetTravels(CancellationToken cancellationToken)
    {
        var travels = await travelQueries.GetAllAsync(cancellationToken);
        return travels.Select(TravelDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<TravelDto>> CreateTravel(
        [FromBody] CreateTravelDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTravelCommand
        {
            Title = request.Title,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Description = request.Description,
            Image = request.Image,
            IsDone = false, // створена подорож ще не завершена
            Places = request.Places,
            Members = request.Members,
            UserId = request.UserId
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<TravelDto>>(
            t => TravelDto.FromDomainModel(t),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<TravelDto>> UpdateTravel(
        [FromBody] UpdateTravelDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTravelCommand
        {
            TravelId = request.Id,
            Title = request.Title,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Description = request.Description,
            Image = request.Image,
            IsDone = request.IsDone,
            Places = request.Places,
            Members = request.Members
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<TravelDto>>(
            t => TravelDto.FromDomainModel(t),
            e => e.ToObjectResult());
    }

    [HttpDelete("{travelId:guid}")]
    public async Task<ActionResult<TravelDto>> DeleteTravel(Guid travelId, CancellationToken cancellationToken)
    {
        var command = new DeleteTravelCommand { TravelId = travelId };
        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<TravelDto>>(
            t => TravelDto.FromDomainModel(t),
            e => e.ToObjectResult());
    }
}
