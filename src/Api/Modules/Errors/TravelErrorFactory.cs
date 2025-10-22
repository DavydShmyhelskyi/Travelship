using Application.Entities.Travels.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class TravelErrorFactory
{
    public static ObjectResult ToObjectResult(this TravelException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                TravelAlreadyExistsException => StatusCodes.Status409Conflict,
                TravelNotFoundException or MembersNotFoundException or PlacesNotFoundException => StatusCodes.Status404NotFound,
                AccessDeniedToTravelException => StatusCodes.Status403Forbidden,
                UnhandledTravelException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Travel error handler not implemented")
            }
        };
}
