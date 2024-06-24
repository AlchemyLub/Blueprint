namespace AlchemyLub.Blueprint.Endpoints.Extensions;

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
    public static IApplicationBuilder UseEndpointsLayer(this IApplicationBuilder app)
    {
        app.UseSwagger(options =>
            options.PreSerializeFilters.Add((swagger, request) =>
                swagger.Servers = new List<OpenApiServer> { new() { Url = $"https://{request.Host}" } }));

        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseResponseCaching();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        return app.UseHealthChecks("/health", new HealthCheckOptions());
    }
}
