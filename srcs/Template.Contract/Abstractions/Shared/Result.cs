namespace Template.Contract.Abstractions.Shared;

public class Result
{
    protected Result(bool isSuccess, Error errors)
    {
        switch (isSuccess)
        {
            case true when errors != Error.None:
                throw new InvalidOperationException("Success result can't contain errors.");
            case false when errors == Error.None:
                throw new InvalidOperationException("Failed result must contain errors.");
            default:
                IsSuccess = isSuccess;
                Errors = errors;
                break;
        }
    }

    public Error Errors { get; set; }
    protected bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, Error.None);
    private static ResultT<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);
    private static ResultT<TValue> Failure<TValue>(Error error) => new(default, false, error);
    protected static ResultT<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}
