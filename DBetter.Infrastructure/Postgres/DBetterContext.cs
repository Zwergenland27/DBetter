using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Routes;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCompositions;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.Users;
using DBetter.Domain.Vehicles;
using DBetter.Infrastructure.OutboxPattern;
using DBetter.Infrastructure.TrainCirculations;
using DBetter.Infrastructure.TrainCompositions;
using DBetter.Infrastructure.TrainRuns;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Postgres;

public class DBetterContext(DbContextOptions<DBetterContext> options) : DbContext(options)
{
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<ConnectionRequest> ConnectionRequests { get; set; }
    
    public DbSet<Connection> Connections { get; set; }
    
    public DbSet<Station> Stations { get; set; }
    
    public DbSet<TrainRunPersistenceDto> TrainRuns { get; set; }
    
    public DbSet<PassengerInformation> PassengerInformation { get; set; }
    
    public DbSet<TrainCirculationPersistenceDto> TrainCirculations { get; set; }
    
    public DbSet<Vehicle> Vehicles { get; set; }
    
    public DbSet<Route> Routes { get; set; }
    
    public DbSet<TrainCompositionPersistenceDto> TrainCompositions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DBetterContext).Assembly);
    }

}