using Application.Entities.Roles.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class RoleErrorFactory
{
    public static ObjectResult ToObjectResult(this RoleException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                RoleAlreadyExistException => StatusCodes.Status409Conflict,
                RoleNotFoundException => StatusCodes.Status404NotFound,
                UnhandledRoleException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Role error handler not implemented")
            }
        };
}
