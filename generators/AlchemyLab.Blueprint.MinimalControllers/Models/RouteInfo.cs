namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Models;

/// <summary>
/// Информация о маршруте
/// </summary>
/// <param name="Template">Шаблон маршрута</param>
/// <param name="Name">Имя маршрута</param>
/// <param name="Order">Положение маршрута при сортировке</param>
internal readonly record struct RouteInfo(
    string? Template,
    string? Name,
    int? Order)
{
    /// <summary>
    /// Создает пустой экземпляр <see cref="RouteInfo"/>
    /// </summary>
    internal static RouteInfo Empty() => new(null, null, null);

    /// <summary>
    /// Создает новый экземпляр <see cref="RouteInfo"/> из данных атрибута
    /// </summary>
    /// <param name="attributeData"><see cref="AttributeData"/></param>
    internal static RouteInfo New(AttributeData attributeData)
    {
        string? template = attributeData.ConstructorArguments.Length > 0
            ? attributeData.ConstructorArguments[0].Value as string
            : null;

        string? name = attributeData.NamedArguments
            .FirstOrDefault(arg => arg.Key is "Name").Value.Value as string;

        int? order = attributeData.NamedArguments
            .FirstOrDefault(arg => arg.Key is "Order").Value.Value as int?;

        return new(CleanTemplate(template), name, order);
    }

    private static string? CleanTemplate(string? template)
    {
        const string actionToken = "[action]";

        if (string.IsNullOrWhiteSpace(template))
        {
            return null;
        }

        //TODO: Удаляем токен [action] из шаблона. Возможно стоит добавить его поддержку в будущем.
        template = template.Replace(actionToken, string.Empty, StringComparison.OrdinalIgnoreCase);

        return !string.IsNullOrEmpty(template)
            ? template
            : null;
    }
}
