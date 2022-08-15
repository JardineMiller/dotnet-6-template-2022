using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.PipelineBehaviours;

namespace Template.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services
    )
    {
        return services
            .AddMediatR(typeof(DependencyInjection).Assembly)
            .AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(RequestValidationBehaviour<,>)
            );
    }
}