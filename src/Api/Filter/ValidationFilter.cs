using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filter
{
    public class ValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments)
            {
                if (argument.Value is null)
                { 
                    continue;
                }
                var argumentType = argument.Value.GetType();
                var validatorType = typeof(FluentValidation.IValidator<>).MakeGenericType(argumentType);

                if(serviceProvider.GetService(validatorType) is IValidator validator)
                {
                    var validationContext = new ValidationContext<object>(argument.Value);
                    var validationResult = await validator.ValidateAsync(validationContext);
                    if (!validationResult.IsValid)
                    {
                        var errors = validationResult.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(e => e.ErrorMessage).ToArray()
                            );
                        context.Result = new BadRequestObjectResult(new ValidationProblemDetails
                        {

                            Errors = errors,
                            Detail = "One or more validation errors occurred.",
                            Title = "ValidationFailed",
                            Status = 400
                        });
                    }
                }
            }
            
            await next();
        }
    }
}
