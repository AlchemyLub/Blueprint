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
