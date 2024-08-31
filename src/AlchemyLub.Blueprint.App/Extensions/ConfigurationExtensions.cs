namespace AlchemyLub.Blueprint.App.Extensions;

/// <summary>
/// Provides extension methods for <inheritdoc cref="IConfiguration"/>
/// </summary>
public static class ConfigurationExtensions
{
    public static string? GetObservabilityUrl(this IConfiguration configuration)
    {
        string configurationPath = ConfigurationPathFactory.CreatePath(
            nameof(ObservabilityOptions),
            nameof(ObservabilityOptions.OpenTelemetryUrl));

        return configuration.GetSection(configurationPath).Value;
    }
}
