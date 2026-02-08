using DBetter.Domain.Routes;
using DBetter.Domain.Routes.Stops;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class RouteMapping : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.ToTable("Routes");
        
        builder.HasKey(r => r.Id);
        
        builder.HasIndex(r => r.TrainRunId)
            .IsUnique();

        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => new RouteId(value));
        
        builder.Property(r => r.TrainRunId)
            .HasConversion(
                id => id.Value,
                value => new TrainRunId(value));

        builder.Property(r => r.LastUpdatedAt);

        builder.Property(r => r.Source);

        builder.OwnsMany(r => r.Stops, sb =>
        {
            sb.ToTable("RouteStops");
            
            sb.WithOwner().HasForeignKey("RouteId");
            
            sb.HasKey(nameof(Stop.Id), "RouteId");

            sb.Property(s => s.Id)
                .HasConversion(
                    id => id.Value,
                    value => new StopId(value));
            
            sb.Property(s => s.StationId)
                .HasConversion(
                    id => id.Value,
                    value => new StationId(value));
            
            sb.OwnsOne(s => s.ArrivalTime);
            
            sb.OwnsOne(s => s.DepartureTime);

            sb.OwnsOne(s => s.Attributes);

        }).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}