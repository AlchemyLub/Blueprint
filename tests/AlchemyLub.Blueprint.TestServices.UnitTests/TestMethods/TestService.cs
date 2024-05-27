namespace AlchemyLub.Blueprint.TestServices.UnitTests.TestMethods;

/// <summary>
/// Контейнер с методами для тестов
/// </summary>
public sealed class TestService
{
    public Task SetName(Guid modelId, string newName) => Task.CompletedTask;

    public void Push(decimal salary, bool isActive)
    {
        return;
    }

    public Task<IEnumerable<string>> Handle(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(Enumerable.Empty<string>());
}
