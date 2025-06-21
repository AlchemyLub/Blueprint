namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Models;

/// <summary>
/// Информация о версии
/// </summary>
/// <param name="Version">Значение версии</param>
/// <param name="Status">Статус версии. Например, значения могут включать "alpha", "beta", "rc" и т.д.</param>
/// <param name="IsDeprecated">Флаг устаревания версии</param>
internal record struct VersionInfo(string Version, string? Status = null, bool IsDeprecated = false)
{
    /// <summary>
    /// Создаёт новый экземпляр <see cref="VersionInfo"/> из данных атрибута ApiVersion
    /// </summary>
    /// <param name="apiVersionAttributeData">Поддерживаемые версии</param>
    internal static VersionInfo NewFromApiVersionAttributeData(AttributeData apiVersionAttributeData)
    {
        ImmutableArray<TypedConstant> constructorArguments = apiVersionAttributeData.ConstructorArguments;

        bool isDeprecated = apiVersionAttributeData.NamedArguments
            .FirstOrDefault(arg => arg.Key == "Deprecated").Value.Value as bool? ?? false;

        if (constructorArguments[0].Value is string stringVersion)
        {
            return new(stringVersion, null, isDeprecated);
        }

        if (constructorArguments[0].Value is not double versionValue)
        {
            throw new InvalidOperationException("Unsupported [ApiVersionAttribute] constructor signature");
        }

        string version = versionValue.ToString("0.0", CultureInfo.InvariantCulture);

        if (constructorArguments[1].Value is string statusValue)
        {
            return new(version, statusValue, isDeprecated);
        }

        return new(version, null, isDeprecated);
    }

    /// <summary>
    /// Создаёт новый экземпляр <see cref="VersionInfo"/> из данных атрибута MapToApiVersion
    /// </summary>
    /// <param name="mapToApiVersionAttributeData">Активные версии</param>
    internal static VersionInfo NewFromMapToApiVersionAttributeData(AttributeData mapToApiVersionAttributeData)
    {
        ImmutableArray<TypedConstant> constructorArguments = mapToApiVersionAttributeData.ConstructorArguments;

        if (constructorArguments.Length is 1)
        {
            return constructorArguments[0].Value switch
            {
                string versionStr => new(versionStr),
                double versionNum => new(versionNum.ToString("0.0")),
                _ => throw new InvalidOperationException("Unsupported MapToApiVersionAttribute constructor argument.")
            };
        }

        throw new InvalidOperationException("Unsupported MapToApiVersionAttribute constructor signature.");
    }
}
