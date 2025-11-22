using DBetter.Application.Abstractions.Persistence;
using DBetter.Application.Requests;
using DBetter.Application.Routes;
using DBetter.Application.Stations;
using DBetter.Application.Users;
using DBetter.Domain.Users;
using DBetter.Infrastructure.ApiMarketplace;
using DBetter.Infrastructure.Authentication;
using DBetter.Infrastructure.BackgroundJobs;
using DBetter.Infrastructure.BahnDe;
using DBetter.Infrastructure.Postgres;
using DBetter.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DBetter.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories();
        services.AddPostgres(configuration);
        services.AddJwtAuthentication(configuration);
        services.AddBahnApi();
        services.AddApiMarketplace(configuration);
        services.AddBackgroundJobs();
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserQueryRepository, UserQueryRepository>();
        services.AddScoped<IStationQueryRepository, StationQueryRepository>();
        services.AddScoped<IConnectionSuggestionService, ConnectionSuggestionService>();
        services.AddScoped<IRouteQueryRepository, RouteQueryRepository>();
        return services;
    }
}