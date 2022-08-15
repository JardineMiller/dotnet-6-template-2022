using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Template.Api.Common.Errors;
using Template.Api.Common.Mapping;

namespace Template.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services
    )
    {
        services.AddControllers();

        return services
            .AddFluentValidation()
            .AddValidatorsFromAssembly(
                Assembly.GetExecutingAssembly()
            )
            .AddMappings()
            .AddSingleton<
                ProblemDetailsFactory,
                ErrorDetailsFactory
            >();
    }
}
