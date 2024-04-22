using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace FloralHub.Blueprint.IntegrationTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
{
    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(t => t.ServiceType == typeof());
        })
    }
}
