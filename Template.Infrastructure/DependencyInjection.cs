using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Interfaces.Services;
using Template.Domain.Entities;
using Template.Infrastructure.Authentication;
using Template.Infrastructure.Email;
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
        services
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddAuth(configuration)
            .AddDatabase(configuration)
            .AddEmail(configuration)
            .AddIdentity();

        return services;
    }

    private static IServiceCollection AddIdentity(
        this IServiceCollection services
    )
    {
        services
            .AddIdentity<User, IdentityRole>(
                options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredUniqueChars = 0;

                    options.SignIn.RequireConfirmedEmail = true;
                }
            )
            .AddTokenProvider<DataProtectorTokenProvider<User>>(
                TokenOptions.DefaultProvider
            )
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }
}
