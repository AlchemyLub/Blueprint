namespace AlchemyLub.Blueprint.Domain.Extensions;

/// <summary>
/// Методы расширения для <see cref="IEnumerable{T}"/>
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Приводит <see cref="IEnumerable{T}"/> к <see cref="List{T}"/>, если возможно, то без итерации
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="source">Исходная коллекция</param>
    public static List<T> AsList<T>(this IEnumerable<T>? source) =>
        source switch
        {
            null => throw new ArgumentNullException(nameof(source)),
            List<T> list => list,
            T[] array => [.. array],
            _ => source.ToList()
        };

    /// <summary>
    /// Приводит <see cref="IEnumerable{T}"/> к <see cref="T:T[]"/>, если возможно, то без итерации
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="source">Исходная коллекция</param>
    public static T[] AsArray<T>(this IEnumerable<T>? source) =>
        source switch
        {
            null => throw new ArgumentNullException(nameof(source)),
            T[] array => array,
            List<T> list => list.ToArray(),
            _ => source.ToArray()
        };

    /// <summary>
    /// Проверяет, является ли коллекция <see langword="null"/> или пустой
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="source">Исходная коллекция</param>
    /// <returns>Возвращает <see langword="true"/>, если коллекция <see langword="null"/> или пуста, иначе <see langword="false"/></returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source) =>
        source switch
        {
            null => true,
            T[] array => array.Length is 0,
            List<T> list => list.Count is 0,
            _ => !source.Any()
        };

    /// <summary>
    /// Применяет фильтрацию к <see cref="IEnumerable{T}"/> только если выполняется заданное условие
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="source">Исходная коллекция</param>
    /// <param name="condition">Условие, при котором фильтрация будет применена</param>
    /// <param name="predicate">Функция фильтрации</param>
    /// <returns>Фильтрованная коллекция, если условие истинно; иначе исходная коллекция</returns>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T>? source, bool condition, Func<T, bool> predicate)
    {
        if (source is null)
        {
            ArgumentNullException.ThrowIfNull(source);
        }

        return condition ? source.FilterWithPredicate(predicate) : source;
    }

    /// <summary>
    /// Фильтрует коллекцию, исключая все элементы равные <see langword="null"/>
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="source">Исходная коллекция</param>
    /// <returns>Коллекция, содержащая только ненулевые элементы</returns>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?>? source) where T : class
    {
        if (source is null)
        {
            ArgumentNullException.ThrowIfNull(source);
        }

        foreach (T? value in source)
        {
            if (value is not null)
            {
                yield return value;
            }
        }
    }

    /// <summary>
    /// Фильтрует коллекцию, исключая все элементы равные <see langword="null"/>
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="source">Исходная коллекция</param>
    /// <returns>Коллекция, содержащая только ненулевые элементы</returns>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?>? source) where T : struct
    {
        if (source is null)
        {
            ArgumentNullException.ThrowIfNull(source);
        }

        foreach (T? value in source)
        {
            if (value is not null)
            {
                yield return value.Value;
            }
        }
    }

    /// <summary>
    /// Применяет фильтрацию к <see cref="IEnumerable{T}"/>, если параметр не равен <see langword="null"/>
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="source">Исходная коллекция</param>
    /// <param name="value">Значение, проверяемое на <see langword="null"/> для фильтрации</param>
    /// <param name="predicate">Функция фильтрации</param>
    /// <returns>Фильтрованная коллекция, если параметр не равен <see langword="null"/>; иначе исходная коллекция</returns>
    public static IEnumerable<T> WhereIfValueNotNull<T>(this IEnumerable<T?>? source, T? value, Func<T?, bool> predicate)
        where T : struct
    {
        if (source is null)
        {
            ArgumentNullException.ThrowIfNull(source);
        }

        return source.WhereIf(value is not null, item => value is not null && predicate(item)).WhereNotNull();
    }

    /// <summary>
    /// Применяет фильтрацию к <see cref="IEnumerable{T}"/>, если параметр не равен <see langword="null"/>
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции</typeparam>
    /// <param name="source">Исходная коллекция</param>
    /// <param name="value">Значение, проверяемое на <see langword="null"/> для фильтрации</param>
    /// <param name="predicate">Функция фильтрации</param>
    /// <returns>Фильтрованная коллекция, если параметр не равен <see langword="null"/>; иначе исходная коллекция</returns>
    public static IEnumerable<T> WhereIfValueNotNull<T>(this IEnumerable<T?>? source, T? value, Func<T?, bool> predicate)
        where T : class
    {
        if (source is null)
        {
            ArgumentNullException.ThrowIfNull(source);
        }

        return source.WhereIf(value is not null, item => value is not null && predicate(item)).WhereNotNull();
    }

    private static IEnumerable<T> FilterWithPredicate<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (var value in source)
        {
            if (predicate(value))
            {
                yield return value;
            }
        }
    }
}
