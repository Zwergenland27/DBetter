using DBetter.Application.Abstractions.Persistence;
using DBetter.Application.Connections;
using DBetter.Application.Stations;
using DBetter.Application.Users;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Users;
using DBetter.Infrastructure.Authentication;
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
        services.AddPostgres("Host=localhost;Database=DBetter;Username=user;Password=password");
        services.AddJwtAuthentication(configuration);
        services.AddBahnApi();
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserQueryRepository, UserQueryRepository>();
        services.AddScoped<IStationQueryRepository, StationQueryRepository>();
        services.AddScoped<IConnectionsQueryRepository, ConnectionsQueryRepository>();
        return services;
    }
}