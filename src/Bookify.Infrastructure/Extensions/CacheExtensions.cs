using Bookify.Application.Abstractions.Caching;
using Bookify.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.Extensions;

internal static class CacheExtensions
{
    public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Cache") ??
                               throw new ArgumentNullException(nameof(configuration));

        services.AddStackExchangeRedisCache(options =>
            options.Configuration = connectionString);

        services.AddSingleton<ICacheService, CacheService>();
    }
}