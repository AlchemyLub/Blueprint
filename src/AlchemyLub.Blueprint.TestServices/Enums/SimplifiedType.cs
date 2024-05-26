namespace AlchemyLub.Blueprint.TestServices.Enums;

/// <summary>
/// Упрощённая типизация
/// </summary>
internal enum SimplifiedType
{
    /// <summary>
    /// Неизвестный тип
    /// </summary>
    /// <remarks>
    /// Указывает на исключительный случай, в корректно работающем коде не должен попадаться
    /// </remarks>
    Unknown = 0,

    /// <summary>
    /// Примитивный тип, находящийся в BCL
    /// </summary>
    /// <remarks>
    /// <b>Необходимые действия:</b> просто сравнить типы
    /// </remarks>
    Primitive = 1,

    /// <summary>
    /// Тип перечисление
    /// </summary>
    /// <remarks>
    /// <b>Необходимые действия:</b> сравнить все значения перечисления
    /// </remarks>
    Enum = 2,

    /// <summary>
    /// Пользовательский тип
    /// </summary>
    /// <remarks>
    /// <b>Необходимые действия:</b> сравнить все свойства типа
    /// </remarks>
    CustomType = 3
}
