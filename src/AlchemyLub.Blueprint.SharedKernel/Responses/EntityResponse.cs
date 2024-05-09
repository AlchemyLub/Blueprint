namespace AlchemyLub.Blueprint.SharedKernel.Responses;

/// <summary>
/// Модель ответа для базовой сущности
/// </summary>
/// <param name="Id">Идентификатор сущности</param>
/// <param name="Title">Заголовок</param>
public record EntityResponse(Guid Id, string Title);
