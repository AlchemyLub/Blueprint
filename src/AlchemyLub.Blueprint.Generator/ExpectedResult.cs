using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlchemyLub.Blueprint.Generator;

public static class ExpectedResult
{
    /// <summary>
    /// Автоматически регистрирует декораторы для сервисов, которые помечены атрибутом <see cref="DecoratorAttribute"/>
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="decoratorsConfigurator">
    /// Конфигуратор декораторов. Можно задать, какие декораторы включены, какие нет. Ключи - имена декораторов, значения - включены или нет.
    /// Если не задан, то все декораторы включены
    /// </param>
    /// <returns></returns>
    public static IServiceCollection AddDecorators(
        this IServiceCollection services,
        Dictionary<string, bool>? decoratorsConfigurator = null)
    {
        if (IsDecoratorEnabled(nameof(Decorator1), decoratorsConfigurator))
        {
            services.Decorate<IService1, Decorator1>();
            services.Decorate<IService2, Decorator1>();
            services.Decorate<IService3, Decorator1>();
        }

        if (IsDecoratorEnabled(nameof(Decorator2), decoratorsConfigurator))
        {
            services.Decorate<IService1, Decorator2>();
            services.Decorate<IService2, Decorator2>();
            services.Decorate<IService3, Decorator2>();
        }

        return services;
    }

    private static bool IsDecoratorEnabled(string decoratorName, Dictionary<string, bool>? decoratorsConfigurator = null)
    {
        if (decoratorsConfigurator is null)
        {
            return true;
        }

        return decoratorsConfigurator.TryGetValue(decoratorName, out bool decoratorEnabled) && decoratorEnabled;
    }

    /// <summary>
    /// Декорирует сервисы, имплементирующие <typeparamref name="TInterface"/>
    /// </summary>
    /// <typeparam name="TInterface">Интерфейс декорируемого сервиса</typeparam>
    /// <typeparam name="TDecorator">Реализация декоратора</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    /// <exception cref="DecoratingServicesNotFoundException"/>
    public static IServiceCollection Decorate<TInterface, TDecorator>(this IServiceCollection services)
        where TInterface : class
        where TDecorator : class, TInterface
    {
        Type interfaceType = typeof(TInterface);

        List<ServiceDescriptor> wrappedDescriptors = services
            .Where(t => t.ServiceType == interfaceType)
            .ToList();

        if (wrappedDescriptors.Count == 0)
        {
            throw new DecoratingServicesNotFoundException(interfaceType);
        }

        foreach (ServiceDescriptor wrappedDescriptor in wrappedDescriptors)
        {
            Func<IServiceProvider, object> factory = CreateFactory(
                typeof(TDecorator),
                wrappedDescriptor);

            services.Replace(ServiceDescriptor.Describe(
                    interfaceType,
                    factory,
                wrappedDescriptor.Lifetime));
        }

        return services;
    }

    private static Func<IServiceProvider, object> CreateFactory(
        Type decoratorType,
        ServiceDescriptor currentDescriptor)
    {
        if (currentDescriptor.ImplementationInstance is not null)
        {
            return serviceProvider => ActivatorUtilities.CreateInstance(
                serviceProvider,
                decoratorType,
                currentDescriptor.ImplementationInstance);
        }

        if (currentDescriptor.ImplementationFactory is not null)
        {
            return serviceProvider => ActivatorUtilities.CreateInstance(
                serviceProvider,
                decoratorType,
                currentDescriptor.ImplementationFactory(serviceProvider));
        }

        if (currentDescriptor.ImplementationType is not null)
        {
            if (decoratorType.IsGenericTypeDefinition)
            {
                return serviceProvider =>
                {
                    object service = ActivatorUtilities.GetServiceOrCreateInstance(
                        serviceProvider,
                        currentDescriptor.ImplementationType);

                    Type[] genericArguments = currentDescriptor.ServiceType.GetGenericArguments();
                    Type closedDecorator = decoratorType.MakeGenericType(genericArguments);

                    return ActivatorUtilities.CreateInstance(
                        serviceProvider,
                        closedDecorator,
                        service);
                };
            }

            return serviceProvider =>
            {
                object service = ActivatorUtilities.GetServiceOrCreateInstance(
                    serviceProvider,
                    currentDescriptor.ImplementationType);

                return ActivatorUtilities.CreateInstance(
                    serviceProvider,
                    decoratorType,
                    service);
            };
        }

        if (currentDescriptor.IsKeyedService)
        {
            if (currentDescriptor.KeyedImplementationInstance is not null)
            {
                return serviceProvider => ActivatorUtilities.CreateInstance(
                    serviceProvider,
                    decoratorType,
                    currentDescriptor.KeyedImplementationInstance);
            }

            if (currentDescriptor.KeyedImplementationFactory is not null)
            {
                return serviceProvider => ActivatorUtilities.CreateInstance(
                    serviceProvider,
                    decoratorType,
                    currentDescriptor.KeyedImplementationFactory(serviceProvider, currentDescriptor.ServiceKey));
            }

            if (currentDescriptor.KeyedImplementationType is not null)
            {
                return serviceProvider =>
                {
                    object service = ActivatorUtilities.CreateInstance(
                        serviceProvider,
                        currentDescriptor.KeyedImplementationType);

                    return ActivatorUtilities.CreateInstance(
                        serviceProvider,
                        decoratorType,
                        service);
                };
            }
        }

        throw new NotImplementedException("Нужная корректная ошибка!");
    }

    public static Func<ServiceDescriptor, bool> CreatePredicate(Type serviceType) =>
        descriptor => descriptor.ServiceType is { IsGenericType: true, IsGenericTypeDefinition: false }
            ? serviceType.GetGenericTypeDefinition() == descriptor.ServiceType.GetGenericTypeDefinition()
            : serviceType == descriptor.ServiceType;

    private static IServiceCollection Replace(
        this IServiceCollection collection,
        ServiceDescriptor descriptor)
    {
        for (int index = 0; index < collection.Count; ++index)
        {
            if (collection[index].ServiceType == descriptor.ServiceType && Equals(collection[index].ServiceKey, descriptor.ServiceKey))
            {
                collection.RemoveAt(index);
                break;
            }
        }
        collection.Add(descriptor);
        return collection;
    }
}
