namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Models;

/// <summary>
/// Информация об авторизации
/// </summary>
/// <param name="Roles"></param>
/// <param name="Policy"></param>
/// <param name="AuthenticationSchemes"></param>
internal readonly record struct AuthInfo(
    string? Roles = null,
    string? Policy = null,
    string? AuthenticationSchemes = null)
{
    /// <summary>
    /// Создает новый экземпляр <see cref="AuthInfo"/> из данных атрибута
    /// </summary>
    /// <param name="attributeData"><see cref="AttributeData"/></param>
    internal static AuthInfo New(AttributeData attributeData)
    {
        string? roles = null;
        string? policy = null;
        string? authSchemes = null;

        foreach (KeyValuePair<string, TypedConstant> namedArg in attributeData.NamedArguments)
        {
            switch (namedArg.Key)
            {
                case "Roles":
                    roles = namedArg.Value.Value as string;
                    break;
                case "Policy":
                    policy = namedArg.Value.Value as string;
                    break;
                case "AuthenticationSchemes":
                    authSchemes = namedArg.Value.Value as string;
                    break;
            }
        }

        return new(roles, policy, authSchemes);
    }

    /// <summary>
    /// Заполнены ли какие-либо параметры
    /// </summary>
    public bool WithParameters =>
        (Roles is not null && Roles.Length > 0)
        || Policy is not null
        || (AuthenticationSchemes is not null && AuthenticationSchemes.Length > 0);
}
