using FluentValidation;
using FluentValidation.Results;
using MediatR;
using POC.ResultsPattern.Abstractions;

namespace POC.ResultsPattern.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequestBase
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
            .Select(validator => validator.Validate(context))
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .ToList();

        if (validationErrors.Count != 0)
        {
            return (TResponse)GetFailureResultResponse(validationErrors)!;
        }

        return await next();
    }

    private static object GetFailureResultResponse(List<ValidationFailure> validationErrors) {

        var responseType = typeof(TResponse);

        if (responseType == typeof(Result))
        {
            return Result.Failure(
                new ValidationError(
                    validationErrors
                        .Select(x => x.ErrorMessage)
                        .ToList()
                )
            );
        }

        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var error = new ValidationError(
                validationErrors
                    .Select(x => x.ErrorMessage)
                    .ToList()
            );

            var genericType = responseType.GetGenericArguments()[0];
            var failureMethod = typeof(Result<>).MakeGenericType(genericType).GetMethod("Failure", new[] { typeof(ValidationError) });
            return failureMethod!.Invoke(null, new object[] { error })!;
        }

        throw new InvalidOperationException("Unsupported response type");
    }
}
