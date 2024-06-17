namespace AlchemyLub.Blueprint.Domain;

/// <summary>
/// Represents the result of an operation, which can be either successful or failed.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Value indicating whether the result is successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <remarks>
    /// Value indicating whether the result is failed.
    /// </remarks>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Error that occurred during the operation, or <see cref="Error.None"/> if the result is successful.
    /// </summary>
    public Error Error { get; }
}

/// <summary>
/// Represents the result of an operation, which can be either successful or failed, and contains a value of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public interface IResult<out T> : IResult
{
    /// <summary>
    /// Gets the value of the result, or <see langword="default"/> if the result is failed.
    /// </summary>
    public T? Value { get; }
}

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
    private Result()
    {
        IsSuccess = true;
        Error = Error.None;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Result"/> with a failed result.
    /// </summary>
    /// <param name="error">The error that occurred during the operation.</param>
    private Result(Error error)
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
    /// Returns a new instance of <see cref="Result"/> that represents a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error that occurred during the operation.</param>
    public static Result Failure(Error error) => new(error);
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
    private Result(Error error)
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
    private Result(T? value)
    {
        IsSuccess = true;
        Error = Error.None;
        Value = value;
    }

    /// <inheritdoc />
    public T? Value { get; }

    /// <inheritdoc />
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
    /// Returns a new instance of <see cref="Result{T}"/> with a successful result and the specified value.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    /// <returns>A new instance of <see cref="Result{T}"/>.</returns>
    [MemberNotNull(nameof(Value))]
    public static Result<T> Success(T value) => new(value);

    /// <summary>
    /// Returns a new instance of <see cref="Result{T}"/> with a failed result and the specified error.
    /// </summary>
    /// <param name="error">The error that occurred during the operation.</param>
    /// <returns>A new instance of <see cref="Result{T}"/>.</returns>
    public static Result<T> Failure(Error error) => new(error);

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> with the message "Invalid result".
    /// </summary>
    [DoesNotReturn]
    private static void FailFast() => throw new ArgumentException("Invalid result");
}
