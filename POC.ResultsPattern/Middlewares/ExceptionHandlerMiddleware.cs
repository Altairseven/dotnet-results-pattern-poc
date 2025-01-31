using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using POC.ResultsPattern.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace POC.ResultsPattern.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate _next, ILogger<ErrorHandlingMiddleware> _logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            context.Response.ContentType = "application/problem+json";

            ProblemDetails details = ex switch
            {
                ValidationException validationEx => new ProblemDetails
                {
                    Title = "Validation Error",
                    Status = 400,
                    Detail = validationEx.Message,
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                },
                InfrastructureException notFoundEx => new ProblemDetails
                {
                    Title = "Infrastructure Error",
                    Status = 500,
                    Detail = notFoundEx.Message,
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                },
                _ => new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Status = 500,
                    Detail = ex.Message,
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                },
            };

            var json = JsonSerializer.Serialize(details);
            await context.Response.WriteAsync(json);
        }
    }
}
