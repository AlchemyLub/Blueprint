namespace AlchemyLab.Blueprint.UseCase
{
    /// <summary>
    /// Атрибут для указания пайплайна, который должен быть применен к UseCase
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ApplyPipelineAttribute : Attribute
    {
        /// <summary>
        /// Создает новый экземпляр атрибута
        /// </summary>
        /// <param name="pipelineType">Тип пайплайна</param>
        public ApplyPipelineAttribute(Type pipelineType)
        {
            if (pipelineType == null)
                throw new ArgumentNullException(nameof(pipelineType));

            PipelineType = pipelineType;
        }

        /// <summary>
        /// Тип пайплайна, который должен быть применен
        /// </summary>
        public Type PipelineType { get; }
    }

    /// <summary>
    /// Атрибут, указывающий, что глобальные пайплайны не должны применяться к данному UseCase
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IgnoreGlobalPipelinesAttribute : Attribute
    {
    }

    /// <summary>
    /// Атрибут для указания, к каким типам UseCase'ов применяется пайплайн
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UseCaseFilterAttribute : Attribute
    {
        /// <summary>
        /// Тип UseCase'а, к которому применяется пайплайн
        /// </summary>
        public Type UseCaseType { get; }

        /// <summary>
        /// Создает новый атрибут фильтра для конкретного UseCase
        /// </summary>
        /// <param name="useCaseType">Тип UseCase'а</param>
        public UseCaseFilterAttribute(Type useCaseType)
        {
            UseCaseType = useCaseType ?? throw new ArgumentNullException(nameof(useCaseType));
        }
    }

    /// <summary>
    /// Атрибут для указания, что пайплайн применяется глобально ко всем UseCase'ам
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GlobalPipelineAttribute : Attribute
    {
    }

    /// <summary>
    /// Атрибут для указания пайплайнов, которые следует применить к UseCase'у
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UsePipelineAttribute : Attribute
    {
        /// <summary>
        /// Тип пайплайна, который нужно применить к UseCase'у
        /// </summary>
        public Type PipelineType { get; }

        /// <summary>
        /// Создает новый атрибут для указания пайплайна
        /// </summary>
        /// <param name="pipelineType">Тип пайплайна</param>
        public UsePipelineAttribute(Type pipelineType)
        {
            PipelineType = pipelineType ?? throw new ArgumentNullException(nameof(pipelineType));

            // Проверяем, что тип реализует интерфейс IPipeline<,>
            var isValidPipeline = pipelineType.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipeline<,>));

            if (!isValidPipeline)
            {
                throw new ArgumentException($"Тип {pipelineType} не реализует интерфейс IPipeline<,>", nameof(pipelineType));
            }
        }
    }
}
