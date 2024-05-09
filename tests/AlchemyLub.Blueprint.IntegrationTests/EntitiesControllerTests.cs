namespace AlchemyLub.Blueprint.IntegrationTests;

// TODO: Если будет время, нужно накатить адекватные тесты.
/// <summary>
/// Тесты для <see cref="EntitiesController"/>
/// </summary>
public class EntitiesControllerTests(IntegrationTestWebAppFactory factory) : BaseTest(factory)
{
    private IEntitiesClient EntitiesClient => RestService.For<IEntitiesClient>(Client);

    /// <summary>
    /// Проверяет корректность ответа при запросе сущности по идентификатору
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetEntity_ShouldBe_SuccessfullyResponse()
    {
        Guid entityId = Guid.NewGuid();

        EntityResponse entityResponse = await EntitiesClient.GetEntity(entityId);

        entityResponse.Id.Should().Be(entityId);
    }

    /// <summary>
    /// Проверяет корректность ответа при создании сущности
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateEntity_ShouldBe_SuccessfullyResponse()
    {
        Guid entityId = await EntitiesClient.CreateEntity();

        entityId.Should().NotBe(Guid.Empty);
    }

    /// <summary>
    /// Проверяет корректность ответа при редактировании сущности
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task UpdateEntity_ShouldBe_SuccessfullyResponse()
    {
        Guid entityId = Guid.NewGuid();
        EntityRequest request = new("Title");

        EntityResponse response = await EntitiesClient.UpdateEntity(entityId, request);

        response.Id.Should().NotBe(Guid.Empty);
        response.Title.Should().Be(request.Title);
    }

    /// <summary>
    /// Проверят корректность ответа при удалении сущности
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteEntity_ShouldBe_SuccessfullyResponse()
    {
        Guid entityId = Guid.NewGuid();

        bool isDeletedSuccessfully = await EntitiesClient.DeleteEntity(entityId);

        isDeletedSuccessfully.Should().BeTrue();
    }
}
