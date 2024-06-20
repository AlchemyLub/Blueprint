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
    public static IApplicationBuilder UseAllLayers(this IApplicationBuilder app) =>
        app
            .UseMiddlewares()
            .UseEndpointsLayer()
            .UseSerilog();

    private static IApplicationBuilder UseSerilog(this IApplicationBuilder app) => app.UseSerilogRequestLogging();

    private static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app) =>
        app
            .UseMiddleware<RequestContextLoggingMiddleware>();
}
