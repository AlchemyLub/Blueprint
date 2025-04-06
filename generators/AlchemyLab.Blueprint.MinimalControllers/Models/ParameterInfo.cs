namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Models;

/// <summary>
/// Информация о параметре метода контроллера
/// </summary>
/// <param name="Name">Название метода контроллера</param>
/// <param name="Type">Тип метода контроллера</param>
/// <param name="IsBody">Является ли параметр частью тела запроса.</param>
internal sealed record ParameterInfo(
    string Name,
    string Type,
    bool IsBody);
