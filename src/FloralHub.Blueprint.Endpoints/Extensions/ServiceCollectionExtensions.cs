namespace FloralHub.Blueprint.Endpoints.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует необходимые сервисы для Presentation слоя.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddEndpointsLayer(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SupportNonNullableReferenceTypes();

            options.AddSecurityDefinition("Bearer", new()
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            options.SwaggerDoc("v1", new()
            {
                Version = "v1",
                Title = $"{nameof(Blueprint)} API",
                Description = "Шаблонный проект",
                Contact = new()
                {
                    Name = "Author",
                    Url = new("https://github.com/VladislavRudakoff"),
                    Email = "vladislav.rudakoff@gmail.com"
                },
                License = new()
                {
                    Name = "License",
                    Url = new("https://mit-license.org/")
                }
            });
        });

        services.AddHealthChecks();

        return services;
    }
}
