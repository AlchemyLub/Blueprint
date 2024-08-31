namespace AlchemyLub.Blueprint.App.Extensions;

/// <summary>
/// Provides extension methods for <inheritdoc cref="IConfiguration"/>
/// </summary>
public static class ConfigurationExtensions
{
    public static Uri? GetObservabilityUrl(this IConfiguration configuration)
    {
        string configurationPath = ConfigurationPathFactory.CreatePath(
            nameof(ObservabilityOptions),
            nameof(ObservabilityOptions.OpenTelemetryUrl));

        Uri.TryCreate(configuration.GetSection(configurationPath).Value, UriKind.Absolute, out Uri? url);

        return url;
    }
}
