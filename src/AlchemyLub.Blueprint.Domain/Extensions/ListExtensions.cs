namespace AlchemyLub.Blueprint.Domain.Extensions;

/// <summary>
/// Методы расширения для <see cref="List{T}"/>
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Добавляет значение в список, если оно не равно <see langword="null"/>
    /// </summary>
    /// <typeparam name="T">Тип элемента листа</typeparam>
    /// <param name="list">Исходный лист</param>
    /// <param name="value">Добавляемое значение</param>
    public static void AddIfNotNull<T>(this List<T> list, T? value)
    {
        if (value is null)
        {
            return;
        }

        list.Add(value);
    }
}
