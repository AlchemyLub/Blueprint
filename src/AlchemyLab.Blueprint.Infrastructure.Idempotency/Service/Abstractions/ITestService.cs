namespace AlchemyLab.Blueprint.Infrastructure.Idempotency.Service.Abstractions;

public interface ITestService
{
    Task<TestResponse> GetTestResponse(TestRequest testRequest);
}
