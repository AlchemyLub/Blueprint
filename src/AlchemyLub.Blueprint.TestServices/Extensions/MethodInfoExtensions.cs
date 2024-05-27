namespace AlchemyLub.Blueprint.TestServices.Extensions;

/// <summary>
/// Методы расширения для <see cref="MethodInfo"/>
/// </summary>
public static class MethodInfoExtensions
{
    /// <summary>
    /// Возвращает параметры метода
    /// </summary>
    /// <param name="methodInfo"><see cref="MethodInfo"/></param>
    /// <returns>Параметры текущего метода</returns>
    internal static MethodParameter[] GetMethodParameters(this MethodInfo methodInfo) =>
        methodInfo
            .GetParameters()
            .Where(t => t.ParameterType != typeof(CancellationToken))
            .Select(t => new MethodParameter(t.ParameterType, t.Name ?? string.Empty))
            .ToArray();

    /// <summary>
    /// Возвращает параметры метода
    /// </summary>
    /// <param name="methodInfo"><see cref="MethodInfo"/></param>
    /// <returns>Параметры текущего метода</returns>
    internal static MethodMetadata ToMethodMetadata(this MethodInfo methodInfo) =>
        new(methodInfo.Name, methodInfo.GetMethodParameters(), methodInfo.ReturnType);

    /// <summary>
    /// Проверяет отсутствие у метода атрибутов автогенерации
    /// </summary>
    /// <param name="methodInfo"><see cref="MethodInfo"/></param>
    /// <returns><see langword="true"/> если найден атрибут автогенерации, <see langword="false"/> если нет</returns>
    internal static bool CheckGeneratedAttributes(this MethodInfo methodInfo) =>
        methodInfo.CustomAttributes.Any(t => t.AttributeType.Name.Contains("Generate"));
}
