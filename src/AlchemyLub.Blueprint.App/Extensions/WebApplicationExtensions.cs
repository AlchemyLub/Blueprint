namespace AlchemyLub.Blueprint.App.Extensions;

/// <summary>
/// Методы расширения для <see cref="WebApplication"/>
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Регистрирует все слои приложения
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/></param>
    /// <returns><see cref="WebApplication"/></returns>
    public static WebApplication UseAllLayers(this WebApplication app) =>
        app.UseEndpointsLayer();
}
