namespace AlchemyLub.Blueprint.Generator.Exceptions;

/// <summary>
/// Исключение, выбрасываемое при отсутствии зарегистрированных имплементаций сервиса
/// </summary>
public class DecoratingServicesNotFoundException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DecoratingServicesNotFoundException"/>
    /// </summary>
    public DecoratingServicesNotFoundException(Type interfaceType)
        : base($"Отсутствуют зарегистрированные имплементации интерфейса {interfaceType}")
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DecoratingServicesNotFoundException"/>
    /// </summary>
    protected DecoratingServicesNotFoundException(string message) : base(message)
    {
    }
}

/// <summary>
/// Исключение, выбрасываемое при отсутствии зарегистрированных имплементаций <typeparamref name="TInterface"/>
/// </summary>
/// <typeparam name="TInterface">Контракт декорируемого сервиса</typeparam>
public sealed class DecoratingServicesNotFoundException<TInterface> : DecoratingServicesNotFoundException
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DecoratingServicesNotFoundException{TInterface}"/>
    /// </summary>
    public DecoratingServicesNotFoundException()
        : base($"Отсутствуют зарегистрированные имплементации интерфейса {typeof(TInterface)}")
    {
    }
}
