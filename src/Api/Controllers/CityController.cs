using Api.Dtos;
using Api.Modules.Errors;
using Application.Entities.Cities.Commands;
using Application.Common.Interfaces.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Services.Abstract;

namespace Api.Controllers;

[Route("cities")]
[ApiController]
public class CitiesController(
    ICityQueries cityQueries,
    ICitiesControllerService controllerService,
    ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CityDto>>> GetCities(CancellationToken cancellationToken)
    {
        var cities = await cityQueries.GetAllAsync(cancellationToken);
        return cities.Select(CityDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CityDto>> Get(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var entity = await controllerService.Get(id, cancellationToken);

        return entity.Match<ActionResult<CityDto>>(
            c => c,
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<CityDto>> CreateCity(
        [FromBody] CreateCityDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCityCommand
        {
            Title = request.Title,
            CountryId = request.CountryId
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<CityDto>>(
            c => CityDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<CityDto>> UpdateCity(
        [FromBody] UpdateCityDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCityCommand
        {
            CityId = request.Id,
            Title = request.Title,
            CountryId = request.CountryId
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<CityDto>>(
            c => CityDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }

    [HttpDelete("{cityId:guid}")]
    public async Task<ActionResult<CityDto>> DeleteCity(
        [FromRoute] Guid cityId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCityCommand { CityId = cityId };
        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<CityDto>>(
            c => CityDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }
}
