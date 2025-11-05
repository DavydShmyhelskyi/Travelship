using Application.Entities.PlacePhotos.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class PlacePhotoErrorFactory
{
    public static ObjectResult ToObjectResult(this PlacePhotoException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                PlacePhotoAlreadyExistException => StatusCodes.Status409Conflict,
                PlacePhotoNotFoundException => StatusCodes.Status404NotFound,
                PlaceNotFoundForPhotoException => StatusCodes.Status404NotFound,
                UnhandledPlacePhotoException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            }
        };
}
