namespace AlchemyLub.Blueprint.App.OptionValidators;

public sealed class OptionsValidator<TOptions> : IValidateOptions<TOptions>
    where TOptions : class
{
    private readonly IServiceProvider serviceProvider;
    private readonly string? sectionName;

    public OptionsValidator(string? name, IServiceProvider serviceProvider) =>
        (sectionName, this.serviceProvider) = (name, serviceProvider);

    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        if (sectionName != null && sectionName != name)
        {
            return ValidateOptionsResult.Skip;
        }

        ArgumentNullException.ThrowIfNull(options);

        using IServiceScope scope = serviceProvider.CreateScope();

        IValidator<TOptions> validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();

        ValidationResult results = validator.Validate(options);

        if (results.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        string typeName = options.GetType().Name;

        List<string> errors = [];

        foreach (ValidationFailure result in results.Errors)
        {
            errors.Add($"Validation failed for '{typeName}.{result.PropertyName}' with the error: '{result.ErrorMessage}'.");
        }

        return ValidateOptionsResult.Fail(errors);
    }
}
