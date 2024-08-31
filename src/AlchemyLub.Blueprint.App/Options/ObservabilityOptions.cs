namespace AlchemyLub.Blueprint.App.Options;

/// <summary>
/// Represents the options for observability.
/// </summary>
public sealed class ObservabilityOptions
{
    /// <summary>
    /// Value indicating whether tracing is enabled.
    /// </summary>
    public required bool TracingIsEnabled { get; set; }

    /// <summary>
    /// Value indicating whether metrics is enabled.
    /// </summary>
    public required bool MetricsIsEnabled { get; set; }

    public required string OpenTelemetryUrl { get; set; }
}
