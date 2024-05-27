namespace AlchemyLub.Blueprint.TestServices.Extensions;

/// <summary>
/// Методы расширения для <see cref="Assembly"/>
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Возвращает все контроллеры
    /// </summary>
    /// <param name="assembly">Текущая сборка</param>
    /// <param name="filter">Фильтр по которому осуществляется выборка из контроллеров</param>
    /// <returns>Отфильтрованные классы контроллеров из текущей сборки</returns>
    public static IEnumerable<Type> GetAllControllersByFilter(
        this Assembly assembly,
        Func<Type, bool> filter) =>
        assembly.GetAllChildTypes<ControllerBase>().Where(filter).ToArray();

    /// <summary>
    /// Возвращает все контроллеры
    /// </summary>
    /// <param name="assembly">Текущая сборка</param>
    /// <returns>Все классы контроллеров из текущей сборки</returns>
    public static IEnumerable<Type> GetAllControllers(this Assembly assembly) => assembly.GetAllChildTypes<ControllerBase>();

    /// <summary>
    /// Возвращает все классы наследуемые от заданного типа
    /// </summary>
    /// <typeparam name="T">Тип родительского класса</typeparam>
    /// <param name="assembly">Текущая сборка</param>
    /// <returns>Все классы контракты из сборки</returns>
    public static IEnumerable<Type> GetAllChildTypes<T>(this Assembly assembly) where T : class
    {
        Type type = typeof(T);

        if (type.IsInterface)
        {
            throw new NotImplementedException("Не используется с интерфейсами! Нужна корректная ошибка");
        }

        return assembly.GetTypes().Where(t => t.IsSubclassOf(type)).ToArray();
    }

    /// <summary>
    /// Возвращает все типы реализующие заданный интерфейс
    /// </summary>
    /// <typeparam name="T">Тип интерфейса, реализации которого необходимо найти в сборке</typeparam>
    /// <param name="assembly">Текущая сборка</param>
    /// <returns>Все типы реализующие заданный интерфейс</returns>
    public static IEnumerable<Type> GetAllTypesThatImplementInterface<T>(this Assembly assembly)
    {
        Type type = typeof(T);

        if (!type.IsInterface)
        {
            throw new NotImplementedException("Используется только с интерфейсами! Нужна корректная ошибка");
        }

        return assembly.GetTypes().Where(t => t.IsAssignableFrom(type));
    }

    /// <summary>
    /// Возвращает все типы, названия которых заканчиваются на заданный постфикс
    /// </summary>
    /// <param name="assembly">Текущая сборка</param>
    /// <param name="postfix">Постфикс по которому будут выбраны типы из заданной сборки</param>
    /// <returns>Все типы, имена которых заканчиваются на заданный постфикс</returns>
    public static IEnumerable<Type> GetAllTypesByPostfix(this Assembly assembly, string postfix)
    {
        Func<Type, bool> func = type => type.FullName?.EndsWith(postfix, StringComparison.InvariantCultureIgnoreCase)
                                        ?? type.Name.EndsWith(postfix, StringComparison.InvariantCultureIgnoreCase);

        return assembly.GetTypes().Where(func);
    }

    /// <summary>
    /// Возвращает все типы, названия которых содержат заданный текст
    /// </summary>
    /// <param name="assembly">Текущая сборка</param>
    /// <param name="text">Текст по которому будут выбраны типы из заданной сборки</param>
    /// <returns>Все типы, имена которых содержат заданный текст</returns>
    public static IEnumerable<Type> GetAllTypesByString(this Assembly assembly, string text)
    {
        Func<Type, bool> func = type => type.FullName?.Contains(text, StringComparison.InvariantCultureIgnoreCase)
                                        ?? type.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase);

        return assembly.GetTypes().Where(func);
    }

    /// <summary>
    /// Возвращает все типы по фильтру
    /// </summary>
    /// <param name="assembly">Текущая сборка</param>
    /// <param name="filter">Фильтр по которому осуществляется выборка из всех типов сборки</param>
    /// <returns>Все типы, имена которых содержат заданный текст</returns>
    public static IEnumerable<Type> GetAllTypesByFilter(this Assembly assembly, Func<Type, bool> filter) =>
        assembly.GetTypes().Where(filter);
}
