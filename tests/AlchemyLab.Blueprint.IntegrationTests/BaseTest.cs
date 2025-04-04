namespace AlchemyLab.Blueprint.IntegrationTests;

public abstract class BaseTest(IntegrationTestWebAppFactory factory) : IClassFixture<IntegrationTestWebAppFactory>
{
    protected HttpClient Client => factory.CreateClient();
}
