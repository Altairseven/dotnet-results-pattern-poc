using MediatR;
using Microsoft.Extensions.Logging;
using POC.ResultsPattern.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using System.Xml.Linq;

namespace POC.ResultsPattern.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequestBase
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation("Executing command {Command}", name);

            var result = await next();

            var (IsSuccess, ErrorMessage) = GetResultStateForLogging(result);
            if(IsSuccess)
                _logger.LogInformation("Command {Command} processed successfully", name);
            else
                _logger.LogError(ErrorMessage, "Command {Command} processing failed", name);

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Command {Command} processing failed", name);

            throw;
        }
    }

    private (bool IsSuccess, string ErrorMessage) GetResultStateForLogging(TResponse result) {
        bool IsSuccess = true;
        string ErrorMessage = "";

        if (result!.GetType() == typeof(Result))
        {
            IsSuccess = (bool)(typeof(Result).GetProperty("IsSuccess")!.GetValue(result)!);
            if (!IsSuccess)
            {
                ErrorMessage = (typeof(Result).GetProperty("ErrorMessage")!.GetValue(result)! as string)!;
            }
        }
        else if (result!.GetType().GetGenericTypeDefinition() == typeof(Result<>))
        {
            var TType = typeof(TResponse).GetGenericArguments()[0];
            IsSuccess = (bool)(typeof(Result<>).MakeGenericType(TType).GetProperty("IsSuccess")!.GetValue(result)!);
            if (!IsSuccess)
            {
                ErrorMessage = (typeof(Result<>).MakeGenericType(TType)
                    .GetProperty("ErrorMessage")!.GetValue(result)! as string)!;
            }
        }
        return (IsSuccess, ErrorMessage);
    }

}