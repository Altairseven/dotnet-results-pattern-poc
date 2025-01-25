using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using MediatR;

namespace POC.ResultsPattern.Abstractions;

public class Result {

    public bool IsSuccess { get; init; }
    public string ErrorMessage { get; init; } = default!;

    public IReadOnlyList<string> ValidationErrors { get; init; } = default!;

    public static Result Success() => new Result(true, default!);
    public static Result Failure(string message) => new Result(false, message);

    public static Result Failure(List<ValidationFailure> validationErrors) => new Result(false, "Invalid property values", MapValidationErrors(validationErrors));

    protected Result(bool isSuccess, string errorMessage, List<string>? validationErrors = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ValidationErrors = validationErrors?.ToList().AsReadOnly() ?? new List<string>().AsReadOnly();
    }

    protected static List<string> MapValidationErrors(List<ValidationFailure> errors) 
    {
        return errors.Select(x => x.ErrorMessage).ToList();
    }
}

public class Result<T> : Result
{
    public T? Value { get; init; }

    public static Result<T> Success(T value = default!) => new Result<T>(true, value, default!);
    public new static Result<T> Failure(string message) => new Result<T>(false, default, message);

    public new static Result<T> Failure(List<ValidationFailure> validationErrors) => 
        new Result<T>(false, default, "Invalid property values", MapValidationErrors(validationErrors));

    private Result(
        bool isSuccess, 
        T? value, 
        string errorMessage, 
        List<string>? validationErrors = null) 
        : base(isSuccess, errorMessage, validationErrors)
    {
        Value = value;
    }
}
