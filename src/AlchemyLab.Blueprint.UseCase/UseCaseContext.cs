using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AlchemyLab.Blueprint.UseCase
{
    /// <summary>
    /// Контекст для выполнения UseCase
    /// </summary>
    public class UseCaseContext
    {
        private IServiceProvider serviceProvider;

        /// <summary>
        /// Инициализирует контекст с указанным провайдером сервисов
        /// </summary>
        public void InitializeServiceProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Инициализирует контекст с текущим провайдером сервисов
        /// </summary>
        public void Initialize()
        {
            if (serviceProvider == null)
                throw new InvalidOperationException("UseCaseContext не может быть инициализирован, так как serviceProvider не установлен.");
        }

        /// <summary>
        /// Получает экземпляр сервиса указанного типа
        /// </summary>
        public T GetService<T>() where T : class
        {
            if (serviceProvider == null)
                throw new InvalidOperationException("UseCaseContext не инициализирован. Необходимо вызвать InitializeServiceProvider() перед использованием.");

            return serviceProvider.GetService<T>();
        }

        /// <summary>
        /// Получает обязательный экземпляр сервиса указанного типа
        /// </summary>
        public T GetRequiredService<T>() where T : class
        {
            if (serviceProvider == null)
                throw new InvalidOperationException("UseCaseContext не инициализирован. Необходимо вызвать InitializeServiceProvider() перед использованием.");

            return serviceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// Выполняет UseCase с указанным запросом
        /// </summary>
        public Task<TResponse> Execute<TRequest, TResponse>(
            TRequest request,
            CancellationToken cancellationToken = default)
            where TRequest : notnull
        {
            if (serviceProvider == null)
                throw new InvalidOperationException("UseCaseContext не инициализирован. Необходимо вызвать InitializeServiceProvider() перед использованием.");

            var useCase = serviceProvider.GetRequiredService<IUseCase<TRequest, TResponse>>();
            return useCase.Execute(request, cancellationToken);
        }

        /// <summary>
        /// Выполняет UseCase с указанным запросом (без возвращаемого значения)
        /// </summary>
        public Task Execute<TRequest>(
            TRequest request,
            CancellationToken cancellationToken = default)
            where TRequest : notnull
        {
            if (serviceProvider == null)
                throw new InvalidOperationException("UseCaseContext не инициализирован. Необходимо вызвать InitializeServiceProvider() перед использованием.");

            var useCase = serviceProvider.GetRequiredService<IUseCase<TRequest>>();
            return useCase.Execute(request, cancellationToken);
        }

        /// <summary>
        /// Применяет пайплайны к запросу и выполняет UseCase
        /// </summary>
        internal Task<TResponse> ApplyPipelines<TRequest, TResponse>(
            TRequest request,
            Func<TRequest, Task<TResponse>> core,
            Type useCaseType,
            CancellationToken cancellationToken = default)
            where TRequest : notnull
        {
            if (serviceProvider == null)
                throw new InvalidOperationException("UseCaseContext не инициализирован. Необходимо вызвать InitializeServiceProvider() перед использованием.");

            // Функция, выполняющая UseCase
            Task<TResponse> ExecuteCore(TRequest req)
            {
                return core(req);
            }

            var registry = serviceProvider.GetRequiredService<UseCasePipelineRegistry>();
            var pipelineTypes = registry.GetPipelineTypes(useCaseType);

            if (pipelineTypes.Count == 0)
            {
                // Если нет пайплайнов, просто выполняем UseCase
                return ExecuteCore(request);
            }

            // Создаем и запускаем пайплайны в обратном порядке
            Func<TRequest, Task<TResponse>> next = ExecuteCore;

            // Пайплайны применяем в обратном порядке (от последнего к первому)
            foreach (var pipelineType in pipelineTypes.OrderByDescending(t => pipelineTypes.IndexOf(t)))
            {
                // Проверяем совместимость типов пайплайна с UseCase
                var pipelineInterfaces = pipelineType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipeline<,>))
                    .ToList();

                var matchingInterface = pipelineInterfaces.FirstOrDefault(i =>
                {
                    var genericArgs = i.GetGenericArguments();
                    return genericArgs.Length == 2 &&
                           genericArgs[0].IsAssignableFrom(typeof(TRequest)) &&
                           typeof(TResponse).IsAssignableFrom(genericArgs[1]);
                });

                if (matchingInterface == null)
                    continue; // Пропускаем несовместимые пайплайны

                // Создаем экземпляр пайплайна
                var pipeline = serviceProvider.GetRequiredService(pipelineType);
                var handleMethod = matchingInterface.GetMethod("HandleAsync");

                var currentNext = next;
                next = async (req) =>
                {
                    var parameters = new object[] { req, currentNext };
                    return (TResponse)await (Task<TResponse>)handleMethod.Invoke(pipeline, parameters);
                };
            }

            return next(request);
        }

        /// <summary>
        /// Применяет пайплайны к запросу и выполняет UseCase (без возвращаемого значения)
        /// </summary>
        internal Task ApplyPipelines<TRequest>(
            TRequest request,
            Func<TRequest, Task<ValueTuple>> core,
            Type useCaseType,
            CancellationToken cancellationToken = default)
            where TRequest : notnull
        {
            if (serviceProvider == null)
                throw new InvalidOperationException("UseCaseContext не инициализирован. Необходимо вызвать InitializeServiceProvider() перед использованием.");

            // Функция, выполняющая UseCase
            Task<ValueTuple> ExecuteCore(TRequest req)
            {
                return core(req);
            }

            var registry = serviceProvider.GetRequiredService<UseCasePipelineRegistry>();
            var pipelineTypes = registry.GetPipelineTypes(useCaseType);

            if (pipelineTypes.Count == 0)
            {
                // Если нет пайплайнов, просто выполняем UseCase
                return ExecuteCore(request).ContinueWith(
                    t => { },
                    cancellationToken,
                    TaskContinuationOptions.None,
                    TaskScheduler.Current);
            }

            // Создаем и запускаем пайплайны в обратном порядке
            Func<TRequest, Task<ValueTuple>> next = ExecuteCore;

            // Пайплайны применяем в обратном порядке (от последнего к первому)
            foreach (var pipelineType in pipelineTypes.OrderByDescending(t => pipelineTypes.IndexOf(t)))
            {
                // Проверяем совместимость типов пайплайна с UseCase
                var pipelineInterfaces = pipelineType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipeline<,>))
                    .ToList();

                var matchingInterface = pipelineInterfaces.FirstOrDefault(i =>
                {
                    var genericArgs = i.GetGenericArguments();
                    return genericArgs.Length == 2 &&
                           genericArgs[0].IsAssignableFrom(typeof(TRequest)) &&
                           typeof(ValueTuple).IsAssignableFrom(genericArgs[1]);
                });

                if (matchingInterface == null)
                    continue; // Пропускаем несовместимые пайплайны

                // Создаем экземпляр пайплайна
                var pipeline = serviceProvider.GetRequiredService(pipelineType);
                var handleMethod = matchingInterface.GetMethod("HandleAsync");

                var currentNext = next;
                next = async (req) =>
                {
                    var parameters = new object[] { req, currentNext };
                    return await (Task<ValueTuple>)handleMethod.Invoke(pipeline, parameters);
                };
            }

            return next(request).ContinueWith(
                t => { },
                cancellationToken,
                TaskContinuationOptions.None,
                TaskScheduler.Current);
        }
    }
}
