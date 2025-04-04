namespace AlchemyLab.Blueprint.Endpoints.Requests;

/// <summary>
/// Модель запроса для базовой сущности
/// </summary>
/// <param name="Title">Заголовок</param>
/// <param name="Description">Описание</param>
/// <param name="Count">Количество</param>
public record EntityRequest(
    string Title,
    string Description,
    int? Count);
