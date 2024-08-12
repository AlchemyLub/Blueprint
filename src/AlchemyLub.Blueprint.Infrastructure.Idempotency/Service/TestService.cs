namespace AlchemyLub.Blueprint.Infrastructure.Idempotency.Service;

public sealed class TestService : ITestService
{
    /// <inheritdoc />
    public Task<TestResponse> GetTestResponse(TestRequest testRequest)
    {

    }
}
