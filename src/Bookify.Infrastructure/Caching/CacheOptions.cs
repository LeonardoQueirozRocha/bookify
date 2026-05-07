using Microsoft.Extensions.Caching.Distributed;

namespace Bookify.Infrastructure.Caching;

public static class CacheOptions
{
    public static DistributedCacheEntryOptions DefaultExpiration =>
        InternalCreate(TimeSpan.FromMinutes(1));

    public static DistributedCacheEntryOptions Create(TimeSpan? expiration) =>
        expiration is not null
            ? InternalCreate(expiration.Value)
            : DefaultExpiration;

    private static DistributedCacheEntryOptions InternalCreate(TimeSpan expiration) =>
        new() { AbsoluteExpirationRelativeToNow = expiration };
}