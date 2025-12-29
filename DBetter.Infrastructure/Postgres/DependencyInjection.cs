using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DBetter.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new PostgreSqlSettings();
        configuration.Bind(PostgreSqlSettings.SectionName, settings);

        services.AddDbContext<DBetterContext>(options =>
        {
            options.UseNpgsql(settings.ConnectionString);
            options.LogTo(Console.WriteLine, LogLevel.Warning);
        });

        return services;
    }
}