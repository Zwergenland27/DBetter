using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Stations;
using DBetter.Domain.Users;
using DBetter.Infrastructure.BahnDe.Routes.Entities;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Postgres;

public class DBetterContext(DbContextOptions<DBetterContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Station> Stations { get; set; }
    
    public DbSet<RouteEntity> Routes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DBetterContext).Assembly);
    }
}