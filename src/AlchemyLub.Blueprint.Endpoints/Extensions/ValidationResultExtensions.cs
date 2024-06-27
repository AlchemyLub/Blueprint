namespace AlchemyLub.Blueprint.Endpoints.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ValidationResult"/>.
/// </summary>
public static class ValidationResultExtensions
{
    /// <summary>
    /// Converts a <see cref="ValidationResult"/> to a dictionary of validation errors.
    /// </summary>
    /// <param name="validationResult">The validation result to convert.</param>
    /// <returns>A dictionary where each key is a property name and each value is an array of error messages.</returns>
    public static Dictionary<string, string[]> ToValidationProblemErrors(this ValidationResult validationResult) =>
        validationResult.Errors
            .GroupBy(validationFailure => validationFailure.PropertyName)
            .ToDictionary(
                validationFailureGrouping => validationFailureGrouping.Key,
                validationFailureGrouping => validationFailureGrouping
                    .Select(validationFailure => validationFailure.ErrorMessage)
                    .ToArray());
}
