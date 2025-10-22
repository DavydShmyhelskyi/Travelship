using Application.Entities.Places.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class PlaceErrorFactory
{
    public static ObjectResult ToObjectResult(this PlaceException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                PlaceAlreadyExistException => StatusCodes.Status409Conflict,
                PlaceNotFoundException => StatusCodes.Status404NotFound,
                UnhandledPlaceException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Place error handler not implemented")
            }
        };
}
