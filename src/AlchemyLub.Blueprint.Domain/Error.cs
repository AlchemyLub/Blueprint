namespace AlchemyLub.Blueprint.Domain;

/// <summary>
/// Represents an error that occurred during an operation.
/// </summary>
/// <param name="Code">Gets the code of the error.</param>
/// <param name="Description">Gets the description of the error, or <see langword="null"/> if no description is provided.</param>
public sealed record Error(string Code, string? Description = null)
{
    /// <summary>
    /// Represents a successful result with no error.
    /// </summary>
    public static readonly Error None = new(string.Empty);
}
