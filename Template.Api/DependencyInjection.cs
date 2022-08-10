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
            .AddMappings()
            .AddSingleton<
                ProblemDetailsFactory,
                ErrorDetailsFactory
            >();
    }
}