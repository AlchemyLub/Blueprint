namespace AlchemyLub.Blueprint.Infrastructure.Database.Extensions;

public static class OptionsExtensions
{
    public static CacheOptions GetMemoryCacheOptions(this IOptionsSnapshot<CacheOptions> cacheOptions) =>
        cacheOptions.Get(CacheOptionNames.MemoryCache);

    public static CacheOptions GetDistributedCacheOptions(this IOptionsSnapshot<CacheOptions> cacheOptions) =>
        cacheOptions.Get(CacheOptionNames.DistributedCache);
}
