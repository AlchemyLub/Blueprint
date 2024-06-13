namespace AlchemyLub.Blueprint.Domain;

// TODO: Комментарии!
public interface IResult
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public Task<IResult> AsTask => Task.FromResult(this);
}

public readonly record struct Result : IResult
{
    private static readonly Result StaticSuccess = new();

    public Result()
    {
        IsSuccess = true;
        Error = Error.None;
    }

    private Result(Error error)
    {
        if (error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = false;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    //public Task<Result> AsTask => Task.FromResult(this);

    public static Result Success() => StaticSuccess;

    public static Result Failure(Error error) => new(error);
}

public readonly record struct Result<T> : IResult
{
    public Result() => FailFast();

    private Result(Error error)
    {
        if (error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = false;
        Error = error;
        Value = default;
    }

    private Result(T value)
    {
        if (value is null)
        {
            throw new ArgumentException("Invalid value", nameof(value));
        }

        IsSuccess = true;
        Value = value;
        Error = Error.None;
    }

    public T? Value { get; }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    //public Task<Result<T>> AsTask => Task.FromResult(this);

    [MemberNotNull(nameof(Value))]
    public static Result<T> Success(T value) => new(value);

    public static Result<T> Failure(Error error) => new(error);

    [DoesNotReturn]
    private static void FailFast() => throw new ArgumentException("Invalid result");
}
