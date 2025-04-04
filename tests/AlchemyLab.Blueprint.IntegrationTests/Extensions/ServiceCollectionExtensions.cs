namespace AlchemyLab.Blueprint.IntegrationTests.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Заменяет текущую реализацию сервиса
    /// </summary>
    /// <typeparam name="TService">Интерфейс сервиса</typeparam>
    /// <typeparam name="TImplementation">Новая имплементация сервиса</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection Replace<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        Type serviceType = typeof(TService);

        ServiceDescriptor? descriptor = services.SingleOrDefault(d => d.ServiceType == serviceType);

        if (descriptor != null)
        {
            services.Replace(ServiceDescriptor.Describe(serviceType, typeof(TImplementation), descriptor.Lifetime));
        }

        return services;
    }
}
