namespace AlchemyLub.Blueprint.Application.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует все сервисы бизнес слоя
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services) =>
        services.AddScoped<IApplicationService, ApplicationService>();
}
