namespace AlchemyLab.Blueprint.Infrastructure.Idempotency.Service;

public sealed class TestService : ITestService
{
    /// <inheritdoc />
    public async Task<TestResponse> GetTestResponse(TestRequest testRequest)
    {
        await Task.CompletedTask;

        return new(testRequest.Id, testRequest.Name);
    }
}
