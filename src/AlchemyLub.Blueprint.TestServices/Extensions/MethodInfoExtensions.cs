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
            .Select(t => new MethodParameter(t.ParameterType, t.Name ?? string.Empty))
            .ToArray();

    /// <summary>
    /// Проверяет отсутствие у метода атрибутов автогенерации
    /// </summary>
    /// <param name="methodInfo"><see cref="MethodInfo"/></param>
    /// <returns><see langword="true"/> если найден атрибут автогенерации, <see langword="false"/> если нет</returns>
    internal static bool CheckGeneratedAttributes(this MethodInfo methodInfo) =>
        methodInfo.CustomAttributes.Any(t => t.AttributeType.Name.Contains("Generate"));
}
