using Application.Entities.Cities.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class CityErrorFactory
{
    public static ObjectResult ToObjectResult(this CityException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                CityAlreadyExistException => StatusCodes.Status409Conflict,
                CityNotFoundException => StatusCodes.Status404NotFound,
                CountryNotFoundForCityException => StatusCodes.Status404NotFound,
                UnhandledCityException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("City error handler not implemented")
            }
        };
}
