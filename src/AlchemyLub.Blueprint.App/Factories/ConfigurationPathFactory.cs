namespace AlchemyLub.Blueprint.App.Factories;

public static class ConfigurationPathFactory
{
    public static string CreatePath(params string[] configurationSections) => string.Join(':', configurationSections);
}
