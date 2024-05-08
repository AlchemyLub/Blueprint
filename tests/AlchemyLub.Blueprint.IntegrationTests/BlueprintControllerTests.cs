namespace AlchemyLub.Blueprint.IntegrationTests;

/// <summary>
/// Тесты для <see cref="BlueprintController"/>
/// </summary>
public class BlueprintControllerTests(IntegrationTestWebAppFactory factory) : BaseTest(factory)
{
    // TODO: Сделать нормальные тесты, если будет время
    [Fact]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
    {
        string url = "/api/Blueprint";

        HttpResponseMessage response = await Client.GetAsync(url);

        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }
}
