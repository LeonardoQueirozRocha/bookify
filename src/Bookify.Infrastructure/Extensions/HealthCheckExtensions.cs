using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.Extensions;

public static class HealthCheckExtensions
{
    public static void AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!)
            .AddRedis(configuration.GetConnectionString("Cache")!)
            .AddUrlGroup(new Uri(configuration["Keycloak:BaseUrl"]!), HttpMethod.Get, "keycloak");
    }

    public static void MapCustomHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}