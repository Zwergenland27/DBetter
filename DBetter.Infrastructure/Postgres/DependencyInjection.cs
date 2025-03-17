using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DBetter.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgres(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DBetterContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.LogTo(Console.WriteLine, LogLevel.Warning);
        });

        return services;
    }
}