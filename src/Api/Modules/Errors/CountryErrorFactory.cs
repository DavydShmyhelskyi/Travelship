using Application.Entities.Countries.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class CountryErrorFactory
{
    public static ObjectResult ToObjectResult(this CountryException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                CountryAlreadyExistException => StatusCodes.Status409Conflict,
                CountryNotFoundException => StatusCodes.Status404NotFound,
                UnhandledCountryException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Country error handler not implemented")
            }
        };
}
