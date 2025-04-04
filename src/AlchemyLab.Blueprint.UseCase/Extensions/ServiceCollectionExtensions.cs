using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using AlchemyLab.Blueprint.UseCase.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace AlchemyLab.Blueprint.UseCase
{
    /// <summary>
    /// Расширения для IServiceCollection для регистрации UseCase и пайплайнов
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует UseCase в контейнере зависимостей
        /// </summary>
        public static IServiceCollection AddUseCase<TUseCase>(this IServiceCollection services)
            where TUseCase : class
        {
            // Регистрируем UseCase как реализацию самого себя
            services.TryAddTransient<TUseCase>();

            // Регистрируем UseCase для всех его вариантов интерфейсов IUseCase
            var useCaseType = typeof(TUseCase);

            foreach (var interfaceType in useCaseType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IUseCase<>))
                {
                    services.TryAddTransient(interfaceType, provider => provider.GetRequiredService<TUseCase>());
                }
                else if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IUseCase<,>))
                {
                    services.TryAddTransient(interfaceType, provider => provider.GetRequiredService<TUseCase>());
                }
            }

            return services;
        }

        /// <summary>
        /// Сканирует сборку и регистрирует все UseCase и пайплайны
        /// </summary>
        public static IServiceCollection AddUseCasesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            // Регистрируем необходимые сервисы
            services.TryAddSingleton<UseCasePipelineRegistry>();

            // Регистрируем все пайплайны
            var pipelineTypes = assembly.GetTypes()
                .Where(type =>
                    !type.IsAbstract &&
                    !type.IsInterface &&
                    type.GetInterfaces().Any(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IPipeline<,>)))
                .ToList();

            foreach (var pipelineType in pipelineTypes)
            {
                services.TryAddTransient(pipelineType);
            }

            // Регистрируем все UseCase
            var useCaseTypes = assembly.GetTypes()
                .Where(type =>
                    !type.IsAbstract &&
                    !type.IsInterface &&
                    (type.BaseType != null && (
                     type.BaseType.IsGenericType &&
                     (type.BaseType.GetGenericTypeDefinition() == typeof(UseCase<>) ||
                      type.BaseType.GetGenericTypeDefinition() == typeof(UseCase<,>)))) ||
                     type.GetInterfaces().Any(i =>
                        i.IsGenericType &&
                        (i.GetGenericTypeDefinition() == typeof(IUseCase<>) ||
                         i.GetGenericTypeDefinition() == typeof(IUseCase<,>))))
                .ToList();

            var addUseCaseMethod = typeof(ServiceCollectionExtensions)
                .GetMethod(nameof(AddUseCase), BindingFlags.Public | BindingFlags.Static);

            foreach (var useCaseType in useCaseTypes)
            {
                var genericMethod = addUseCaseMethod.MakeGenericMethod(useCaseType);
                genericMethod.Invoke(null, new object[] { services });
            }

            // Сканируем сборку для нахождения атрибутов пайплайнов
            var registry = CreateRegistry(services);
            registry.ScanAssembly(assembly);

            return services;
        }

        /// <summary>
        /// Создает провайдер сервисов для UseCase с инициализацией контекста
        /// </summary>
        public static ServiceProvider BuildUseCaseProvider(this IServiceCollection services)
        {
            // Получаем ServiceProvider с помощью стандартного метода расширения
            var serviceProvider = services.BuildServiceProvider();

            // Если в сервисах зарегистрирован UseCaseContext, инициализируем его
            var context = serviceProvider.GetService<UseCaseContext>();

            if (context != null)
            {
                context.Initialize();
            }

            return serviceProvider;
        }

        /// <summary>
        /// Добавляет пайплайн в коллекцию сервисов
        /// </summary>
        public static IServiceCollection AddPipeline<TPipeline>(this IServiceCollection services)
            where TPipeline : class
        {
            var pipelineType = typeof(TPipeline);
            services.AddTransient(pipelineType);

            // Получаем все интерфейсы IPipeline<,>, которые реализует данный тип
            foreach (var interfaceType in pipelineType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipeline<,>)))
            {
                services.AddTransient(interfaceType, pipelineType);
            }

            // Получаем реестр пайплайнов или создаем новый, если его еще нет
            var registry = CreateRegistry(services);

            // Регистрируем пайплайн в реестре
            if (pipelineType.IsDefined(typeof(GlobalPipelineAttribute), true))
            {
                registry.RegisterGlobalPipeline(pipelineType);
            }

            return services;
        }

        /// <summary>
        /// Добавляет пайплайн специально для указанного UseCase
        /// </summary>
        public static IServiceCollection AddPipelineFor<TPipeline, TUseCase>(this IServiceCollection services)
            where TPipeline : class
        {
            var pipelineType = typeof(TPipeline);
            var useCaseType = typeof(TUseCase);

            services.AddTransient(pipelineType);

            // Получаем все интерфейсы IPipeline<,>, которые реализует данный тип
            foreach (var interfaceType in pipelineType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipeline<,>)))
            {
                services.AddTransient(interfaceType, pipelineType);
            }

            // Получаем реестр пайплайнов или создаем новый, если его еще нет
            var registry = CreateRegistry(services);

            // Регистрируем пайплайн для указанного UseCase
            registry.RegisterPipelineForUseCase(pipelineType, useCaseType);

            return services;
        }

        /// <summary>
        /// Регистрирует валидаторы из указанной сборки
        /// </summary>
        public static IServiceCollection AddValidatorsFromAssembly(
            this IServiceCollection services,
            Assembly assembly)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            // Находим все типы, реализующие IValidator<>
            var validatorTypes = assembly.GetTypes()
                .Where(type =>
                    !type.IsAbstract &&
                    !type.IsInterface &&
                    type.GetInterfaces().Any(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(Pipelines.IValidator<>)))
                .ToList();

            // Регистрируем каждый валидатор
            foreach (var validatorType in validatorTypes)
            {
                // Находим все интерфейсы IValidator<>, которые реализует валидатор
                var validatorInterfaces = validatorType.GetInterfaces()
                    .Where(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(Pipelines.IValidator<>))
                    .ToList();

                // Регистрируем валидатор для каждого интерфейса
                foreach (var validatorInterface in validatorInterfaces)
                {
                    services.AddScoped(validatorInterface, validatorType);
                }
            }

            return services;
        }

        /// <summary>
        /// Создает экземпляр реестра пайплайнов
        /// </summary>
        private static UseCasePipelineRegistry CreateRegistry(IServiceCollection services)
        {
            var registry = new UseCasePipelineRegistry();
            services.AddSingleton(registry);
            return registry;
        }
    }

    /// <summary>
    /// Интерфейс для конфигурации пайплайнов UseCase
    /// </summary>
    public interface IUseCasePipelinesBuilder
    {
        /// <summary>
        /// Добавляет глобальный пайплайн, который будет применяться ко всем UseCase
        /// </summary>
        IUseCasePipelinesBuilder AddGlobalPipeline<TPipeline>() where TPipeline : class;

        /// <summary>
        /// Добавляет пайплайн для конкретного UseCase
        /// </summary>
        IUseCasePipelinesBuilder AddPipeline<TUseCase, TPipeline>()
            where TUseCase : class
            where TPipeline : class;

        /// <summary>
        /// Возвращает текущую коллекцию сервисов
        /// </summary>
        IServiceCollection Services { get; }
    }

    /// <summary>
    /// Реализация построителя конфигурации пайплайнов
    /// </summary>
    internal class UseCasePipelinesBuilder : IUseCasePipelinesBuilder
    {
        private readonly Dictionary<Type, HashSet<Type>> pipelineMap = new();

        public UseCasePipelinesBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public IUseCasePipelinesBuilder AddGlobalPipeline<TPipeline>()
            where TPipeline : class
        {
            var pipelineType = typeof(TPipeline);

            // Регистрируем пайплайн в DI-контейнере
            Services.TryAddTransient(pipelineType);

            // Получаем или создаем реестр пайплайнов
            var registry = CreateRegistry(Services);

            // Регистрируем пайплайн как глобальный
            registry.RegisterGlobalPipeline(pipelineType);

            return this;
        }

        public IUseCasePipelinesBuilder AddPipeline<TUseCase, TPipeline>()
            where TUseCase : class
            where TPipeline : class
        {
            var useCaseType = typeof(TUseCase);
            var pipelineType = typeof(TPipeline);

            // Регистрируем пайплайн в DI-контейнере
            Services.TryAddTransient(pipelineType);

            // Получаем или создаем реестр пайплайнов
            var registry = CreateRegistry(Services);

            // Регистрируем пайплайн для конкретного UseCase
            registry.RegisterPipelineForUseCase(pipelineType, useCaseType);

            return this;
        }

        private static UseCasePipelineRegistry CreateRegistry(IServiceCollection services)
        {
            // Ищем зарегистрированный реестр или создаем новый
            var descriptor = services.FirstOrDefault(d =>
                d.ServiceType == typeof(UseCasePipelineRegistry));

            if (descriptor != null)
            {
                if (descriptor.ImplementationInstance != null)
                    return (UseCasePipelineRegistry)descriptor.ImplementationInstance;

                return new UseCasePipelineRegistry();
            }

            var registry = new UseCasePipelineRegistry();
            services.AddSingleton(registry);
            return registry;
        }
    }
}
