using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.Users;
using DBetter.Infrastructure.OutboxPattern;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Postgres;

public class DBetterContext(DbContextOptions<DBetterContext> options) : DbContext(options)
{
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<ConnectionRequest> ConnectionRequests { get; set; }
    
    public DbSet<Connection> Connections { get; set; }
    
    public DbSet<Station> Stations { get; set; }
    
    public DbSet<TrainRun> TrainRuns { get; set; }
    
    public DbSet<PassengerInformation> PassengerInformation { get; set; }
    
    public DbSet<TrainCirculation> TrainCirculations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DBetterContext).Assembly);
    }

}