namespace AlchemyLub.Blueprint.TestServices.Extensions;

/// <summary>
/// Методы расширения для <see cref="FieldInfo"/>
/// </summary>
public static class FieldInfoExtensions
{
    /// <summary>
    /// Получает константное значение поля перечисления [<see langword="enum"/>]
    /// </summary>
    /// <param name="fieldInfo"><see cref="FieldInfo"/></param>
    /// <returns>Константное значение поля перечисления [<see langword="enum"/>]</returns>
    internal static int GetEnumConstantValue(this FieldInfo fieldInfo)
    {
        int constantValue = Convert.ToInt32(fieldInfo.GetRawConstantValue());

        return constantValue;
    }
}
