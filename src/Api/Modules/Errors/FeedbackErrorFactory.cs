using Application.Entities.Feedbacks.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class FeedbackErrorFactory
{
    public static ObjectResult ToObjectResult(this FeedbackException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                FeedbackAlreadyExistException => StatusCodes.Status409Conflict,
                FeedbackNotFoundException => StatusCodes.Status404NotFound,
                UnhandledFeedbackException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Feedback error handler not implemented")
            }
        };
}
