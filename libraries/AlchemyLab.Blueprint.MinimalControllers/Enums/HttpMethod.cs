namespace AlchemyLab.Blueprint.MinimalControllers.Enums;

/// <summary>
/// Перечисление, представляющее методы HTTP
/// </summary>
public enum HttpMethod
{
    /// <summary>
    /// Неизвестный метод. Используется, когда метод не распознан или не поддерживается
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Метод GET запрашивает представление указанного ресурса. Запросы с использованием метода GET должны получать только данные
    /// </summary>
    GET = 1,

    /// <summary>
    /// Метод POST используется для отправки данных на сервер. Данные, отправленные с помощью метода POST, помещаются в тело запроса
    /// </summary>
    POST = 2,

    /// <summary>
    /// Метод PUT заменяет все текущие представления ресурса данными запроса. Если ресурс не существует, сервер может создать его с помощью метода PUT
    /// </summary>
    PUT = 3,

    /// <summary>
    /// Метод PATCH используется для частичного обновления ресурса. В отличие от PUT, который заменяет весь ресурс, PATCH вносит изменения только в указанные поля
    /// </summary>
    PATCH = 4,

    /// <summary>
    /// Метод DELETE удаляет указанный ресурс. После успешного выполнения запроса ресурс больше не доступен
    /// </summary>
    DELETE = 5,

    /// <summary>
    /// Метод HEAD запрашивает заголовки указанного ресурса. Он аналогичен методу GET, но не возвращает тело ответа
    /// </summary>
    HEAD = 6,

    /// <summary>
    /// Метод OPTIONS используется для описания параметров связи с ресурсом. Он позволяет клиенту узнать, какие методы HTTP поддерживаются сервером для данного ресурса
    /// </summary>
    OPTIONS = 7
}
