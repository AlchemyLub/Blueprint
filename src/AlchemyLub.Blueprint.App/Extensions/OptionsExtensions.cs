namespace AlchemyLub.Blueprint.App.Extensions;

public static class OptionsExtensions
{
    public static OptionsBuilder<TOptions> AddOptionsWithValidation<TOptions, TValidator>(this IServiceCollection services)
        where TOptions : class
        where TValidator : class, IValidator<TOptions>
    {
        services.AddSingleton<IValidator<TOptions>, TValidator>();

        string configurationSectionName = typeof(TOptions).Name;

        return services
            .AddOptions<TOptions>()
            .BindConfiguration(configurationSectionName)
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
