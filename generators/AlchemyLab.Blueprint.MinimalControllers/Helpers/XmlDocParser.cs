namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Helpers;

/// <summary>
/// Утилита для парсинга XML-документации
/// </summary>
public static class XmlDocParser
{
    private static readonly Regex CleanupRegex = new(@"\s+", RegexOptions.Compiled);

    /// <summary>
    /// Модель данных для хранения всей извлеченной XML-документации
    /// </summary>
    public class XmlDocumentation
    {
        /// <summary>
        /// Текст из тега summary
        /// </summary>
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// Словарь описаний параметров, где ключ - имя параметра
        /// </summary>
        public Dictionary<string, string> ParamDescriptions { get; } = [];

        /// <summary>
        /// Описание возвращаемого значения
        /// </summary>
        public string Returns { get; set; } = string.Empty;

        /// <summary>
        /// Коллекция примеров кода
        /// </summary>
        public List<string> Examples { get; } = [];

        /// <summary>
        /// Коллекция исключений с их описаниями
        /// </summary>
        public Dictionary<string, string> Exceptions { get; } = [];

        /// <summary>
        /// Пометки об устаревании (Obsolete)
        /// </summary>
        public string Obsolete { get; set; } = string.Empty;

        /// <summary>
        /// Текст из тега remarks
        /// </summary>
        public string Remarks { get; set; } = string.Empty;
    }

    /// <summary>
    /// Извлекает всю информацию из XML-документации
    /// </summary>
    /// <param name="xmlDocumentation">XML-документация</param>
    /// <returns>Объект с извлеченной документацией</returns>
    public static XmlDocumentation ParseDocumentation(string xmlDocumentation)
    {
        XmlDocumentation result = new();

        if (string.IsNullOrWhiteSpace(xmlDocumentation))
        {
            return result;
        }

        try
        {
            string xmlContent = xmlDocumentation.Trim().StartsWith("<")
                ? xmlDocumentation
                : $"<root>{xmlDocumentation}</root>";

            XDocument doc = XDocument.Parse(xmlContent);

            XElement? summary = doc.Descendants(XmlDocParameters.Summary).FirstOrDefault();
            if (summary is not null)
            {
                result.Summary = CleanText(summary.Value);
            }

            foreach (XElement parameter in doc.Descendants(XmlDocParameters.Param))
            {
                string? paramName = parameter.Attribute(XmlDocParameters.Name)?.Value;
                if (paramName is not null)
                {
                    result.ParamDescriptions[paramName] = CleanText(parameter.Value);
                }
            }

            XElement? returns = doc.Descendants(XmlDocParameters.Returns).FirstOrDefault();
            if (returns is not null)
            {
                result.Returns = CleanText(returns.Value);
            }

            foreach (XElement example in doc.Descendants(XmlDocParameters.Example))
            {
                result.Examples.Add(CleanText(example.Value));
            }

            foreach (XElement exception in doc.Descendants(XmlDocParameters.Exception))
            {
                string? exceptionType = exception.Attribute(XmlDocParameters.Cref)?.Value;

                if (exceptionType is null)
                {
                    continue;
                }

                // Удаляем префикс T: из cref
                exceptionType = exceptionType.StartsWith("T:")
                    ? exceptionType[2..]
                    : exceptionType;

                result.Exceptions[exceptionType] = CleanText(exception.Value);
            }

            XElement? obsolete = doc.Descendants(XmlDocParameters.Obsolete).FirstOrDefault();
            if (obsolete is not null)
            {
                result.Obsolete = CleanText(obsolete.Value);
            }

            XElement? remarks = doc.Descendants(XmlDocParameters.Remarks).FirstOrDefault();
            if (remarks is not null)
            {
                result.Remarks = CleanText(remarks.Value);
            }

            return result;
        }
        catch (Exception)
        {
            return result;
        }
    }

    /// <summary>
    /// Извлекает текст из тега summary
    /// </summary>
    /// <param name="xmlDocumentation">XML-документация</param>
    /// <returns>Текст из тега summary или пустая строка</returns>
    public static string GetSummary(string xmlDocumentation) => ParseDocumentation(xmlDocumentation).Summary;

    /// <summary>
    /// Очищает текст от лишних пробелов
    /// </summary>
    /// <param name="text">Исходный текст</param>
    /// <returns>Очищенный текст</returns>
    private static string CleanText(string text) =>
        string.IsNullOrWhiteSpace(text)
            ? string.Empty
            : CleanupRegex.Replace(text.Trim(), " ");
}
