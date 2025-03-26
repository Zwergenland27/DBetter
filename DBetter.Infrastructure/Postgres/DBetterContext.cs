using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.Entities;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Stations;
using DBetter.Domain.Users;
using DBetter.Infrastructure.BahnDe.Connections.Entities;
using DBetter.Infrastructure.BahnDe.TrainRuns.Entities;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Postgres;

public class DBetterContext(DbContextOptions<DBetterContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Station> Stations { get; set; }
    
    public DbSet<ConnectionRequest> ConnectionRequests { get; set; }
    
    public DbSet<BahnConnectionRequestEntity> BahnConnectionRequests { get; set; }
    
    public DbSet<TrainRunEntity> TrainRuns { get; set; }
    
    public DbSet<ConnectionEntity> Connections { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DBetterContext).Assembly);
    }
}