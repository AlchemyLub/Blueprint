namespace AlchemyLub.Blueprint.TestServices.Extensions;

/// <summary>
/// Методы расширения для <see cref="Assembly"/>
/// </summary>
public static class AssemblyExtensions
{
    //private const string Controller = nameof(Controller);
    private const string Client = nameof(Client);

    /// <summary>
    /// Возвращает все контроллеры
    /// </summary>
    /// <param name="assembly">Текущая сборка</param>
    /// <returns>Все классы контроллеров из текущей сборки</returns>
    public static Type[] GetAllControllers(this Assembly assembly)
    {
        Type controllerBaseType = typeof(ControllerBase);

        return assembly.GetTypes().Where(t => t.IsSubclassOf(controllerBaseType)).ToArray();
    }

    /// <summary>
    /// Возвращает все классы наследуемые от заданного типа
    /// </summary>
    /// <typeparam name="T">Тип родительского класса</typeparam>
    /// <param name="assembly">Текущая сборка</param>
    /// <returns>Все классы контракты из сборки</returns>
    public static Type[] GetAllChildTypes<T>(this Assembly assembly) where T : class
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
    public static Type[] GetAllTypesThatImplementInterface<T>(this Assembly assembly)
    {
        Type type = typeof(T);

        if (!type.IsInterface)
        {
            throw new NotImplementedException("Используется только с интерфейсами! Нужна корректная ошибка");
        }

        return assembly.GetTypes().Where(t => t.IsAssignableFrom(type)).ToArray();
    }

    // TODO: Описать методы, которые позволят корректно получать нужные типы контрактов и сделать дефолты, чтобы можно было ничего не указывать
}
