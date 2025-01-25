using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using POC.ResultsPattern.Abstractions;
using System.Collections.Generic;

namespace POC.ResultsPattern.Extensions;

/// <summary>
/// Extensions to map Result Pattern class to Http Results.
/// They are separated from the actual Result class cause it will usually live in an Application layer project and this methods are only needed in the Api project
/// </summary>
public static class ResultMappingExtensions
{
    public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<ProblemDetails, TOut> onFailure
    )
    {
        return result.IsSuccess ? onSuccess() : onFailure(
            new ProblemDetails 
            {
                Title = "Bad Request",
                Status = 400,
                Detail = result.ErrorMessage,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            }
        );
    }

    public static TOut Match<Tin, TOut>(
        this Result<Tin> result,
        Func<Tin, TOut> onSuccess,
        Func<ProblemDetails, TOut> onFailure
    )
    {
        if (!result.IsSuccess) {
            var extensions = new Dictionary<string, object?>();
            if (result.ValidationErrors.Count > 0)
            {
                extensions.Add("validation_errors", result.ValidationErrors);
            }

            return onFailure(
                new ProblemDetails
                {
                    Title = "Bad Request",
                    Status = 400,
                    Detail = result.ErrorMessage,
                    Extensions = extensions,
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                }
            );
        }

        return onSuccess(result.Value!);
    }
}
