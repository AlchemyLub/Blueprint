using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AlchemyLab.Blueprint.UseCase
{
    /// <summary>
    /// Реестр пайплайнов для UseCase
    /// </summary>
    internal class UseCasePipelineRegistry
    {
        private readonly ConcurrentDictionary<Type, HashSet<Type>> useCasePipelines = new();
        private readonly HashSet<Type> globalPipelines = new();

        /// <summary>
        /// Регистрирует пайплайн как глобальный (применяется ко всем UseCase)
        /// </summary>
        public void RegisterGlobalPipeline(Type pipelineType)
        {
            if (pipelineType == null)
                throw new ArgumentNullException(nameof(pipelineType));

            lock (globalPipelines)
            {
                globalPipelines.Add(pipelineType);
            }
        }

        /// <summary>
        /// Регистрирует пайплайн для конкретного UseCase
        /// </summary>
        public void RegisterPipelineForUseCase(Type pipelineType, Type useCaseType)
        {
            if (pipelineType == null)
                throw new ArgumentNullException(nameof(pipelineType));
            if (useCaseType == null)
                throw new ArgumentNullException(nameof(useCaseType));

            var pipelines = useCasePipelines.GetOrAdd(useCaseType, _ => new HashSet<Type>());

            lock (pipelines)
            {
                pipelines.Add(pipelineType);
            }
        }

        /// <summary>
        /// Сканирует сборку для поиска пайплайнов с атрибутами и регистрирует их
        /// </summary>
        public void ScanAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            // Находим все пайплайны с атрибутом GlobalPipelineAttribute
            var globalPipelineTypes = assembly.GetTypes()
                .Where(type =>
                    !type.IsAbstract &&
                    !type.IsInterface &&
                    type.GetInterfaces().Any(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IPipeline<,>)) &&
                    type.GetCustomAttribute<GlobalPipelineAttribute>() != null)
                .ToList();

            foreach (var pipelineType in globalPipelineTypes)
            {
                RegisterGlobalPipeline(pipelineType);
            }

            // Находим все UseCase с атрибутом ApplyPipelineAttribute
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

            foreach (var useCaseType in useCaseTypes)
            {
                var pipelineAttributes = useCaseType.GetCustomAttributes<ApplyPipelineAttribute>().ToList();

                foreach (var attr in pipelineAttributes)
                {
                    RegisterPipelineForUseCase(attr.PipelineType, useCaseType);
                }
            }
        }

        /// <summary>
        /// Возвращает список типов пайплайнов для указанного UseCase
        /// </summary>
        public List<Type> GetPipelineTypes(Type useCaseType)
        {
            if (useCaseType == null)
                throw new ArgumentNullException(nameof(useCaseType));

            var result = new List<Type>();

            // Добавляем специфичные пайплайны для UseCase
            if (useCasePipelines.TryGetValue(useCaseType, out var pipelines))
            {
                result.AddRange(pipelines);
            }

            // Добавляем глобальные пайплайны, если UseCase не имеет атрибута IgnoreGlobalPipelinesAttribute
            var ignoreGlobal = useCaseType.GetCustomAttribute<IgnoreGlobalPipelinesAttribute>() != null;

            if (!ignoreGlobal)
            {
                lock (globalPipelines)
                {
                    result.AddRange(globalPipelines);
                }
            }

            return result;
        }
    }
}
