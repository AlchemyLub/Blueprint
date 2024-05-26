namespace AlchemyLub.Blueprint.TestServices.Extensions;

/// <summary>
/// Методы расширения для <see cref="Assembly"/>
/// </summary>
public static class AssemblyExtensions
{
    private const string Controller = nameof(Controller);
    private const string Client = nameof(Client);

    /// <summary>
    /// Возвращает все типы наследуемые от заданного типа
    /// </summary>
    /// <param name="assembly">Текущая сборка</param>
    /// <returns>Все классы контракты из сборки</returns>
    public static Type[] GetAllChildTypes<T>(this Assembly assembly) =>
        assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(T))).ToArray();

    // TODO: Описать методы, которые позволят корректно получать нужные типы контрактов и сделать дефолты, чтобы можно было ничего не указывать
}
