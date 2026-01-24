using CleanMessageBus.Abstractions.DependencyInjection;
using CleanMessageBus.RabbitMQ.DependencyInjection;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Application.Users;
using DBetter.Domain.Abstractions;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Routes;
using DBetter.Domain.Stations;
using DBetter.Domain.Users;
using DBetter.Infrastructure.ApiMarketplace;
using DBetter.Infrastructure.Authentication;
using DBetter.Infrastructure.BahnDe;
using DBetter.Infrastructure.OutboxPattern;
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
        services.AddOutbox();
        services.AddJwtAuthentication(configuration);
        services.AddBahnApi();
        services.AddApiMarketplace(configuration);
        services.AddCleanMessageBus(cfg =>
        {
            cfg.RegisterDomainEventsFromAssembly(typeof(AggregateRoot<>).Assembly);
            cfg.RegisterHandlersFromAssembly(typeof(Application.DependencyInjection).Assembly);
            cfg.UseRabbitMq(config =>
            {
                config.WithHostname("localhost");
            });
        });
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserQueryRepository, UserQueryRepository>();
        services.AddScoped<IPassengerInformationRepository, PassengerInformationRepository>();
        services.AddScoped<IStationExternalProvider, ExternalStationProvider>();
        services.AddScoped<IRouteRepository, RouteRepository>();
        services.AddScoped<IStationRepository, StationRepository>();
        services.AddScoped<IConnectionRequestRepository, ConnectionRequestRepository>();
        services.AddScoped<IConnectionRepository, ConnectionRepository>();
        return services;
    }
}