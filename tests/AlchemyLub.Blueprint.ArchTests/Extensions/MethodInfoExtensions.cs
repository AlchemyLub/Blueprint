namespace AlchemyLub.Blueprint.ArchTests.Extensions;

/// <summary>
/// Методы расширения для <see cref="MethodInfo"/>
/// </summary>
public static class MethodInfoExtensions
{
    /// <summary>
    /// Проверяет отсутствие у метода атрибутов автогенерации
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <returns></returns>
    internal static bool CheckGeneratedAttributes(this MethodInfo methodInfo) =>
        methodInfo.CustomAttributes.Any(t => t.AttributeType.Name.Contains("Generate"));
}
