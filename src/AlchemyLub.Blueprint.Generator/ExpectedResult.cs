using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace AlchemyLub.Blueprint.Generator;

public static class ExpectedResult
{
    ///// <summary>
    ///// Автоматически регистрирует декораторы для сервисов, которые помечены атрибутом <see cref="DecoratorAttribute"/>
    ///// </summary>
    ///// <param name="services"><see cref="IServiceCollection"/></param>
    ///// <param name="decoratorsConfigurator">
    ///// Конфигуратор декораторов. Можно задать, какие декораторы включены, какие нет.
    ///// Ключи - имена декораторов, значения - включены или нет. Если <see langword="null"/>, то все декораторы по умолчанию выключены
    ///// </param>
    //public static IServiceCollection AddDecorators(
    //    this IServiceCollection services,
    //    Dictionary<string, bool>? decoratorsConfigurator = null)
    //{
    //    if (IsDecoratorEnabled(nameof(Decorator1), decoratorsConfigurator))
    //    {
    //        services.Decorate<IService1, Decorator1>();
    //        services.Decorate<IService2, Decorator1>();
    //        services.Decorate<IService3, Decorator1>();
    //    }

    //    if (IsDecoratorEnabled(nameof(Decorator2), decoratorsConfigurator))
    //    {
    //        services.Decorate<IService1, Decorator2>();
    //        services.Decorate<IService2, Decorator2>();
    //        services.Decorate<IService3, Decorator2>();
    //    }

    //    return services;
    //}

    private static bool IsDecoratorEnabled(string decoratorName, Dictionary<string, bool>? decoratorsConfigurator = null)
    {
        if (decoratorsConfigurator is null)
        {
            return false;
        }

        return decoratorsConfigurator.TryGetValue(decoratorName, out bool decoratorEnabled) && decoratorEnabled;
    }
}
