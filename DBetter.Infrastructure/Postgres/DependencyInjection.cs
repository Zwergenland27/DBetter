using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DBetter.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgres(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DBetterContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}