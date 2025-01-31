namespace POC.ResultsPattern.Abstractions;

public class Result {

    public bool IsSuccess { get; init; }
    public string ErrorMessage { get; init; } = default!;

    public IReadOnlyList<string> ValidationErrors { get; init; } = default!;

    public static Result Success() => new Result(true, default!);
    public static Result Failure(Error error) => new Result(false, MapErrorMessage(error));

    public static Result Failure(ValidationError error) => new Result(false, MapErrorMessage(error), error.ValidationErrors);

    protected Result(bool isSuccess, string errorMessage, List<string>? validationErrors = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ValidationErrors = validationErrors?.ToList().AsReadOnly() ?? new List<string>().AsReadOnly();
    }

    protected static string MapErrorMessage(Error error) {
        return $"{error.Code} - {error.Name}";
    }
}

public class Result<T> : Result
{
    public T? Value { get; init; }

    public static Result<T> Success(T value = default!) => new Result<T>(true, value, default!);
    public new static Result<T> Failure(Error error) => new Result<T>(false, default, MapErrorMessage(error));

    public new static Result<T> Failure(ValidationError error) => 
        new Result<T>(false, default, MapErrorMessage(error), error.ValidationErrors);

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
