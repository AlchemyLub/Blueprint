namespace AlchemyLub.Blueprint.Domain.Extensions;

/// <summary>
/// Extensions for <see cref="Result"/> and <see cref="Result{T}"/>.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Matches the result with either a success or failure function.
    /// </summary>
    /// <typeparam name="TOut">The type of the output.</typeparam>
    /// <param name="result">The result to match.</param>
    /// <param name="onSuccess">The function to call if the result is successful.</param>
    /// <param name="onFailure">The function to call if the result is a failure.</param>
    /// <returns>The result of the matched function.</returns>
    public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<Error, TOut> onFailure)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return result.IsSuccess
            ? onSuccess()
            : onFailure(result.Error);
    }

    /// <summary>
    /// Matches the result with either a success or failure function.
    /// </summary>
    /// <typeparam name="TIn">The type of the input.</typeparam>
    /// <typeparam name="TOut">The type of the output.</typeparam>
    /// <param name="result">The result to match.</param>
    /// <param name="onSuccess">The function to call if the result is successful.</param>
    /// <param name="onFailure">The function to call if the result is a failure.</param>
    /// <returns>The result of the matched function.</returns>
    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<Error, TOut> onFailure)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result.Error);
    }
}
