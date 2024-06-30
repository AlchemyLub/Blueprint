namespace AlchemyLub.Blueprint.App.OptionValidators;

public sealed class CacheOptionsValidator : AbstractValidator<CacheOptions>
{
    public CacheOptionsValidator()
    {
        When(options => options.IsEnabled, () =>
        {
            RuleFor(t => t.CacheStore)
                .IsInEnum()
                .NotEqual(CacheStore.None);

            RuleFor(t => t.IsAbsoluteExpiration)
                .Equal(true)
                .When(t => !t.IsSlidingExpiration);

            RuleFor(t => t.IsSlidingExpiration)
                .Equal(true)
                .When(t => !t.IsAbsoluteExpiration);

            RuleFor(t => t.CacheDuration)
                .GreaterThan(0);
        });
    }
}
