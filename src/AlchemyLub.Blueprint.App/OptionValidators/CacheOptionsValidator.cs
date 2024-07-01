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

            RuleFor(t => t)
                .Must((options, _) => options.IsSlidingExpiration != options.IsAbsoluteExpiration)
                .WithMessage("[IsSlidingExpiration] and [IsAbsoluteExpiration] cannot be turned on or off at the same time");

            RuleFor(t => t.CacheDuration)
                .GreaterThan(0);
        });
    }
}
