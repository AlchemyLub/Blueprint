namespace AlchemyLub.Blueprint.App.Extensions;

public static class OptionsExtensions
{
    public static OptionsBuilder<TOptions> AddOptionsWithValidation<TOptions, TValidator>(
        this IServiceCollection services,
        string name)
        where TOptions : class
        where TValidator : class, IValidator<TOptions>
    {
        services.TryAddSingleton<IValidator<TOptions>, TValidator>();

        string configurationSectionName = typeof(TOptions).Name;

        OptionsBuilder<TOptions> optionsBuilder = new(services, name);

        services.Configure<TOptions>(name, options =>
        {
            IConfiguration configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            configuration.GetSection($"{configurationSectionName}:{name}").Bind(options);
        });

        return optionsBuilder
            .AutoValidate()
            .ValidateOnStart();
    }

    private static OptionsBuilder<TOptions> AutoValidate<TOptions>(
        this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(provider =>
            new OptionsValidator<TOptions>(optionsBuilder.Name, provider));

        return optionsBuilder;
    }
}
