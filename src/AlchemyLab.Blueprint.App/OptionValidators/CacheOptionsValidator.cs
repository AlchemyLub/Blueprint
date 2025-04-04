namespace AlchemyLab.Blueprint.App.OptionValidators;

public sealed class CacheOptionsValidator : AbstractValidator<CacheOptions>
{
    public CacheOptionsValidator() =>
        When(options => options.IsEnabled, () =>
        {
            RuleFor(t => t.CacheStore)
                .IsInEnum()
                .NotEqual(CacheStore.None);

            RuleFor(t => t.CacheDuration)
                .GreaterThan(0);
        });
}
