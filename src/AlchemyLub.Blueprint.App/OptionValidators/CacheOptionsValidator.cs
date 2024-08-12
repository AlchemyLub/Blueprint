namespace AlchemyLub.Blueprint.App.OptionValidators;

public sealed class CacheOptionsValidator : AbstractValidator<CacheOptions>
{
    private const string ExpirationErrorMessage = $"[{nameof(CacheOptions.IsSlidingExpiration)}]" +
                                                  $" and [{nameof(CacheOptions.IsAbsoluteExpiration)}]" +
                                                  $" cannot be turned on or off at the same time";

    public CacheOptionsValidator() =>
        When(options => options.IsEnabled, () =>
        {
            RuleFor(t => t.CacheStore)
                .IsInEnum()
                .NotEqual(CacheStore.None);

            RuleFor(t => t)
                .Must((options, _) => options.IsSlidingExpiration != options.IsAbsoluteExpiration)
                .WithMessage(ExpirationErrorMessage);

            RuleFor(t => t.CacheDuration)
                .GreaterThan(0);
        });
}
