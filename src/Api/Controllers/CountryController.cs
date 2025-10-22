using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Entities.Countries.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("countries")]
[ApiController]
public class CountriesController(
    ICountryQueries countryQueries,
    ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CountryDto>>> GetCountries(CancellationToken cancellationToken)
    {
        var countries = await countryQueries.GetAllAsync(cancellationToken);
        return countries.Select(CountryDto.FromDomainModel).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<CountryDto>> CreateCountry(
        [FromBody] CreateCountryDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCountryCommand { Title = request.Title };
        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<CountryDto>>(
            c => CountryDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<CountryDto>> UpdateCountry(
        [FromBody] UpdateCountryDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCountryCommand
        {
            CountryId = request.Id,
            Title = request.Title
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<CountryDto>>(
            c => CountryDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }

    [HttpDelete("{countryId:guid}")]
    public async Task<ActionResult<CountryDto>> DeleteCountry(Guid countryId, CancellationToken cancellationToken)
    {
        var command = new DeleteCountryCommand { CountryId = countryId };
        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<CountryDto>>(
            c => CountryDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }
}
