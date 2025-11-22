using System.Reflection;
using DBetter.Application.Abstractions.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace DBetter.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediator();

        return services;
    }
    
    private static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            options.AddOpenBehavior(typeof(TransactionalPipelineBehaviour<,>));
        });

        return services;
    }
}