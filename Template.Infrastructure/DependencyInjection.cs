using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Interfaces.Authentication;
using Template.Application.Common.Interfaces.Persistence;
using Template.Application.Common.Interfaces.Services;
using Template.Infrastructure.Authentication;
using Template.Infrastructure.Persistence;
using Template.Infrastructure.Services;

namespace Template.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName)
        );

        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddSingleton<IJwtGenerator, JwtGenerator>()
            .AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}