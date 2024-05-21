namespace AlchemyLub.Blueprint.Clients.Requests;

/// <summary>
/// Модель запроса для базовой сущности
/// </summary>
/// <param name="Title">Заголовок</param>
/// <param name="Description">Описание</param>
/// <param name="Test">Тест</param>
public record EntityRequest(string Title, string Description, IEnumerable<Test> Test); // TODO: 3й параметр только для тестов!

// TODO: Просто для тестов, потом - удалить!
public record GenericTest<T>(T Value);

// TODO: Просто для тестов, потом - удалить!
public record Test(DateTime Time, string Top);
