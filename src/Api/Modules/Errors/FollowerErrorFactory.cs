using Application.Entities.Followers.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class FollowerErrorFactory
{
    public static ObjectResult ToObjectResult(this Exception error)
        => error switch
        {
            FollowerAlreadyExistException => new ObjectResult(error.Message)
            { StatusCode = StatusCodes.Status409Conflict },

            FollowerNotFoundException => new ObjectResult(error.Message)
            { StatusCode = StatusCodes.Status404NotFound },

            FollowerUserNotFoundException => new ObjectResult(error.Message)
            { StatusCode = StatusCodes.Status404NotFound },

            FollowedUserNotFoundException => new ObjectResult(error.Message)
            { StatusCode = StatusCodes.Status404NotFound },

            CannotFollowYourselfException => new ObjectResult(error.Message)
            { StatusCode = StatusCodes.Status400BadRequest },

            InvalidFollowerOperationException => new ObjectResult(error.Message)
            { StatusCode = StatusCodes.Status400BadRequest },

            UnhandledFollowerException => new ObjectResult(error.Message)
            { StatusCode = StatusCodes.Status500InternalServerError },

            _ => throw new NotImplementedException("Follower error handler not implemented")
        };
}
