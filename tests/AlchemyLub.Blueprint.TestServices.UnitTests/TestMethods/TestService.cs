namespace AlchemyLub.Blueprint.TestServices.UnitTests.TestMethods;

public sealed class TestService
{
    public void Push(decimal salary, bool isActive) { }

    public async Task<IEnumerable<string>> Handle(Guid id, CancellationToken cancellationToken = default) =>
        await Task.FromResult(Enumerable.Empty<string>());

    public Task SetName(Guid modelId, string? newName) => Task.CompletedTask;

    private async Task Handle() => await Task.CompletedTask;
}

public sealed class SameTestService
{
    public Task SetName(Guid modelId, string newName) => Task.CompletedTask;

    public void Push(decimal salary, bool isActive) { }

    public Task<IEnumerable<string>> Handle(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(Enumerable.Empty<string>());
}

public sealed class WrongTestService
{
    public void SetName(Guid id, string name) { }

    public Task Push(object? message) => Task.CompletedTask;

    public Task<IEnumerable<Guid>> Handle(Guid id, string externalId, CancellationToken cancellationToken = default) =>
        Task.FromResult(Enumerable.Empty<Guid>());
}
