using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCompositions;
using DBetter.Domain.TrainCompositions.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Domain.Vehicles.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class TrainCompositionMapping : IEntityTypeConfiguration<TrainComposition>
{
    public void Configure(EntityTypeBuilder<TrainComposition> builder)
    {
        builder.ToTable("TrainCompositions");
        
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.TrainRun)
            .IsUnique();

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new TrainCompositionId(value));
        
        builder.Property(x => x.TrainRun)
            .HasConversion(
                id => id.Value,
                value => new TrainRunId(value));

        builder.Property(x => x.Source);

        builder.OwnsMany(x => x.Vehicles, vb =>
        {
            vb.ToTable("FormationVehicles");
            
            vb.WithOwner().HasForeignKey("TrainCompositionId");
            
            vb.HasKey("TrainCompositionId", nameof(FormationVehicle.Id));

            vb.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,
                    value => new FormationVehicleId(value));

            vb.Property(x => x.FromStation)
                .HasConversion(
                    id => id.Value,
                    value => new StationId(value));
            
            vb.Property(x => x.ToStation)
                .HasConversion(
                    id => id.Value,
                    value => new StationId(value));
            
            vb.Property(x => x.VehicleId)
                .HasConversion(
                    id => id.Value,
                    value => new VehicleId(value));

        }).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}