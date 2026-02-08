using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Abstractions.Interfaces;
using Bookify.Domain.Apartments.Interfaces;
using Bookify.Domain.Bookings.Interfaces;
using Bookify.Domain.Users.Interfaces;
using Bookify.Infrastructure.Clock;
using Bookify.Infrastructure.Context;
using Bookify.Infrastructure.Data;
using Bookify.Infrastructure.Email;
using Bookify.Infrastructure.Repositories;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddPersistenceConfiguration(configuration);
        services.AddAuthentication(configuration);
        services.AddAuthorization();
        
        return services;
    }
}