namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Builders;

/// <summary>
/// Строитель строк с поддержкой отступов
/// </summary>
public sealed class IndentedStringBuilder
{
    private readonly StringBuilder stringBuilder = new();
    private readonly string indentString;
    private bool indentPending = true;

    /// <summary>
    /// Создает новый экземпляр строителя строк с отступами
    /// </summary>
    /// <param name="indentSize">Размер отступа (количество пробелов)</param>
    public IndentedStringBuilder(int indentSize = 4) => indentString = new(' ', indentSize);

    /// <summary>
    /// Текущий уровень отступа
    /// </summary>
    public int IndentLevel { get; private set; }

    /// <summary>
    /// Увеличивает уровень отступа
    /// </summary>
    /// <returns>Текущий экземпляр строителя для цепочки вызовов</returns>
    public IndentedStringBuilder Indent()
    {
        IndentLevel++;
        return this;
    }

    /// <summary>
    /// Уменьшает уровень отступа
    /// </summary>
    /// <returns>Текущий экземпляр строителя для цепочки вызовов</returns>
    public IndentedStringBuilder Outdent()
    {
        if (IndentLevel > 0)
        {
            IndentLevel--;
        }
        return this;
    }

    /// <summary>
    /// Добавляет строку с текущим отступом
    /// </summary>
    /// <param name="value">Строка для добавления</param>
    /// <returns>Текущий экземпляр строителя для цепочки вызовов</returns>
    public IndentedStringBuilder Append(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (indentPending && value.Length > 0)
        {
            for (int i = 0; i < IndentLevel; i++)
            {
                stringBuilder.Append(indentString);
            }
            indentPending = false;
        }

        stringBuilder.Append(value);
        return this;
    }

    /// <summary>
    /// Добавляет строку с текущим отступом и переходом на новую строку
    /// </summary>
    /// <param name="value">Строка для добавления</param>
    /// <returns>Текущий экземпляр строителя для цепочки вызовов</returns>
    public IndentedStringBuilder AppendLine(string? value = null)
    {
        if (value is not null)
        {
            Append(value);
        }

        stringBuilder.AppendLine();
        indentPending = true;
        return this;
    }

    /// <summary>
    /// Добавляет пустую строку
    /// </summary>
    /// <returns>Текущий экземпляр строителя для цепочки вызовов</returns>
    public IndentedStringBuilder AppendBlankLine()
    {
        stringBuilder.AppendLine();
        indentPending = true;
        return this;
    }

    /// <summary>
    /// Добавляет строку с блоком кода, ограниченным фигурными скобками
    /// </summary>
    /// <param name="content">Функция, генерирующая содержимое блока</param>
    /// <param name="openBrace">Открывающая фигурная скобка</param>
    /// <param name="closeBrace">Закрывающая фигурная скобка</param>
    /// <returns>Текущий экземпляр строителя для цепочки вызовов</returns>
    public IndentedStringBuilder AppendBlock(Action<IndentedStringBuilder> content, string openBrace = "{", string closeBrace = "}")
    {
        AppendLine(openBrace);
        Indent();
        content(this);
        Outdent();
        AppendLine(closeBrace);
        return this;
    }

    /// <summary>
    /// Возвращает строковое представление построенной строки
    /// </summary>
    /// <returns>Построенная строка</returns>
    public override string ToString() => stringBuilder.ToString();
}
