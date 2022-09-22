using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
        services
            .AddAuth(configuration)
            .AddDatabase(configuration)
            .AddScoped<IUserRepository, UserRepository>()
            .AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    private static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));

        services.AddSingleton<IJwtGenerator, JwtGenerator>();

        services
            .AddAuthentication(
                defaultScheme: JwtBearerDefaults.AuthenticationScheme
            )
            .AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        jwtSettings.Secret
                                    )
                                ),
                        };
                }
            );

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var databaseSettings = new DatabaseSettings();
        configuration.Bind(
            DatabaseSettings.SectionName,
            databaseSettings
        );
        services.AddSingleton(Options.Create(databaseSettings));

        services
            .AddDbContext<ApplicationDbContext>(
                options =>
                {
                    options.UseSqlServer(
                        connectionString: databaseSettings.ConnectionString
                    );
                }
            )
            .ApplyMigrations();

        return services;
    }

    private static IServiceCollection ApplyMigrations(
        this IServiceCollection services
    )
    {
        var dbContext = services
            .BuildServiceProvider()
            .GetRequiredService<ApplicationDbContext>();

        dbContext?.Database.Migrate();

        return services;
    }

    // private static IServiceCollection AddIdentity(
    //     this IServiceCollection services,
    //     IConfiguration configuration
    // )
    // {
    //     services
    //         .AddIdentityCore<User>()
    //         .AddEntityFrameworkStores<ApplicationDbContext>();
    //
    //     services.Configure<IdentityOptions>(
    //         options =>
    //         {
    //             options.Password.RequireDigit = false;
    //             options.Password.RequiredLength = 6;
    //             options.Password.RequireLowercase = false;
    //             options.Password.RequireNonAlphanumeric = false;
    //             options.Password.RequireUppercase = false;
    //             options.Password.RequiredUniqueChars = 0;
    //         }
    //     );
    //
    //     return services;
    // }
}
