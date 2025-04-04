namespace AlchemyLab.Blueprint.Domain;

/// <summary>
/// Represents the result of an operation, which can be either successful or failed.
/// </summary>
public record Result : IResult
{
    /// <summary>
    /// A static instance of <see cref="Result"/> that represents a successful result.
    /// </summary>
    private static readonly Result StaticSuccess = new();

    /// <summary>
    /// Initializes a new instance of <see cref="Result"/> with a successful result.
    /// </summary>
    internal Result()
    {
        IsSuccess = true;
        Error = Error.None;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Result"/> with a failed result.
    /// </summary>
    /// <param name="error">The error that occurred during the operation.</param>
    internal Result(Error error)
    {

        if (error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = false;
        Error = error;
    }

    /// <inheritdoc />
    public bool IsSuccess { get; }

    /// <inheritdoc />
    public Error Error { get; }

    /// <summary>
    /// Asynchronously returns the current instance as a task.
    /// </summary>
    public Task<Result> AsTask() => Task.FromResult(this);

    /// <summary>
    /// Returns a static instance of <see cref="Result"/> that represents a successful result.
    /// </summary>
    public static Result Success() => StaticSuccess;

    /// <summary>
    /// Returns a new instance of <see cref="Result{T}"/> with a successful result and the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value of the result.</param>
    /// <returns>A new instance of <see cref="Result{T}"/>.</returns>
    public static Result<T> Success<T>(T value) => new(value);

    /// <summary>
    /// Returns a new instance of <see cref="Result"/> that represents a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error that occurred during the operation.</param>
    public static Result Failure(Error error) => new(error);

    /// <summary>
    /// Returns a new instance of <see cref="Result{T}"/> with a failed result and the specified error.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="error">The error that occurred during the operation.</param>
    /// <returns>A new instance of <see cref="Result{T}"/>.</returns>
    public static Result<T> Failure<T>(Error error) => new(error);
}

/// <inheritdoc cref="IResult{T}"/>
public readonly record struct Result<T> : IResult<T>
{
    /// <summary>
    /// Initializes a new instance of <see cref="Result{T}"/> with a failed result.
    /// </summary>
    public Result() => FailFast();

    /// <summary>
    /// Initializes a new instance of <see cref="Result{T}"/> with a failed result and the specified error.
    /// </summary>
    /// <param name="error">The error that occurred during the operation.</param>
    internal Result(Error error)
    {
        if (error == Error.None)
        {
            throw new ArgumentException("Invalid result", nameof(error));
        }

        IsSuccess = false;
        Error = error;
        Value = default;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Result{T}"/> with a successful result and the specified value.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    internal Result(T? value)
    {
        IsSuccess = true;
        Error = Error.None;
        Value = value;
    }

    /// <inheritdoc />
    public T? Value { get; }


    /// <inheritdoc />
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the error that occurred during the operation, or <see cref="Error.None"/> if the result is successful.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Asynchronously returns the current instance as a task.
    /// </summary>
    public Task<Result<T>> AsTask() => Task.FromResult(this);

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> with the message "Invalid result".
    /// </summary>
    [DoesNotReturn]
    private static void FailFast() => throw new ArgumentException("Invalid result");
}
