using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Template.Api.Common.Errors;
using Template.Api.Common.Mapping;
using Template.Application.Common.Interfaces.Misc;

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
            .AddValidatorsFromAssemblyContaining<IAssemblyMarker>()
            .AddMappings()
            .AddSingleton<
                ProblemDetailsFactory,
                ErrorDetailsFactory
            >();
    }
}