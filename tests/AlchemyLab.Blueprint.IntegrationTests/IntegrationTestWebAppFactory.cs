namespace AlchemyLab.Blueprint.IntegrationTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
{
    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureTestServices(services =>
        {
            services.AddBlueprintClients();

            services.Replace<IEntityRepository, EntityRepositoryStub>();
        });
}
