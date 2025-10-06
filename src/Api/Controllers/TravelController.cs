using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Entities.Travels.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("travels")]
[ApiController]
public class TravelsController(
    ITravelQueries travelQueries,
    IValidator<CreateTravelDto> createTravelDtoValidator,
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
        var validationResult = createTravelDtoValidator.Validate(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var input = new CreateTravelCommand
        {
            Title = request.Title,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Description = request.Description,
            Image = request.Image,
            IsDone = request.IsDone,
            UserId = request.UserId
        };

        var newTravel = await sender.Send(input, cancellationToken);
        return TravelDto.FromDomainModel(newTravel);
    }
}
