using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Entities.Cities.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("cities")]
[ApiController]
public class CitiesController(
    ICityQueries cityQueries,
    IValidator<CreateCityDto> createCityDtoValidator,
    ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CityDto>>> GetCities(CancellationToken cancellationToken)
    {
        var cities = await cityQueries.GetAllAsync(cancellationToken);
        return cities.Select(CityDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<CityDto>> CreateCity(
        [FromBody] CreateCityDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = createCityDtoValidator.Validate(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var input = new CreateCityCommand
        {
            Title = request.Title,
            CountryId = request.CountryId
        };

        var newCity = await sender.Send(input, cancellationToken);
        return CityDto.FromDomainModel(newCity);
    }
}
