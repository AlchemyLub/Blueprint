namespace AlchemyLub.Blueprint.Infrastructure.S3.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует все сервисы инфраструктурного слоя(S3)
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddS3InfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddServices()
            .AddMinio(client =>
            {
                client.WithEndpoint(configuration["AWS:ServiceURL"]);
                client.WithCredentials(configuration["AWS:AccessKey"], configuration["AWS:SecretKey"]);
                client.WithSSL(false);
                client.WithRegion(configuration["AWS:Region"]);
            });

    private static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddScoped<IS3Service, S3Service>();
}
