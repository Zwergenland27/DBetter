using DBetter.Domain.Stations;
using DBetter.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Postgres;

public class DBetterContext(DbContextOptions<DBetterContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Station> Stations { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DBetterContext).Assembly);
    }
}