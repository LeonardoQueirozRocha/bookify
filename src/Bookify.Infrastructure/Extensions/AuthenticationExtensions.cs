using Bookify.Application.Abstractions.Authentication;
using Bookify.Infrastructure.Authentication.DelegatingHandlers;
using Bookify.Infrastructure.Authentication.Options;
using Bookify.Infrastructure.Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AuthenticationOptions = Bookify.Infrastructure.Authentication.Options.AuthenticationOptions;
using AuthenticationService = Bookify.Infrastructure.Authentication.Services.AuthenticationService;
using IAuthenticationService = Bookify.Application.Abstractions.Authentication.IAuthenticationService;

namespace Bookify.Infrastructure.Extensions;

internal static class AuthenticationExtensions
{
    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));
        services.AddTransient<AdminAuthorizationDelegatingHandler>();
        services
            .AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpClient) =>
                httpClient.BaseAddress = new Uri(
                    serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value.AdminUrl))
            .AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
            httpClient.BaseAddress = new Uri(
                serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value.TokenUrl));
    }
}