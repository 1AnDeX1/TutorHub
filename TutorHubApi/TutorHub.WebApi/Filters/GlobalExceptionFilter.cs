using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TutorHub.BusinessLogic.Exceptions;

namespace TutorHub.WebApi.Filters;

public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var innerException = exception.InnerException?.ToString() ?? "None";

        if (exception is ValidationException validationException)
        {
            logger.LogWarning("Validation error: {Message}", validationException.Message);

            context.Result = new ObjectResult(new { Message = validationException.Message })
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Value = exception.Message,
            };
        }
        else if (exception is NotFoundException notFoundException)
        {
            logger.LogWarning("Not found error: {Message}", notFoundException.Message);

            context.Result = new ObjectResult(new { Message = notFoundException.Message })
            {
                StatusCode = StatusCodes.Status404NotFound,
                Value = exception.Message,
            };
        }
        else
        {
            logger.LogError(
                exception,
                "Unhandled exception occurred: {ExceptionMessage}. Inner Exception: {InnerException}. StackTrace: {StackTrace}",
                exception.Message,
                innerException,
                exception.StackTrace);

            context.Result = new ObjectResult(new { Message = "An unexpected error occurred. Please try again later." })
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Value = exception.Message,
            };
        }

        context.ExceptionHandled = true;
    }
}