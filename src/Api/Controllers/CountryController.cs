using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Entities.Countries.Commands;
using FluentValidation;
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

        var input = new CreateCountryCommand
        {
            Title = request.Title
        };

        var newCountry = await sender.Send(input, cancellationToken);
        return CountryDto.FromDomainModel(newCountry);
    }
}
