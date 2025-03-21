using DBetter.Domain.Journey;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRun.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class TrainRunMapping : IEntityTypeConfiguration<TrainRunEntity>
{
    public void Configure(EntityTypeBuilder<TrainRunEntity> builder)
    {
        builder.ToTable("TrainRuns");
        
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.BahnId)
            .IsUnique();
        
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new TrainRunId(value));

        builder.Property(x => x.BahnId)
            .HasConversion(
                id => id.Value,
                value => new BahnJourneyId(value));
        
        builder.Property(x => x.Date)
            .HasConversion(
                date => date.Value,
                value => new  TrainRunDate(value));
        
        builder.OwnsOne(x => x.TrainInfos, tib =>
        {
            tib.Property(x => x.Product);

            tib.Property(x => x.Line)
                .HasConversion(
                    line => line != null ? line.Value : null,
                    value => value != null ? new TrainLine(value) : null)
                .IsRequired(false);
            
            tib.Property(x => x.Number)
                .HasConversion(
                    number => number != null ? number.Value : (int?) null,
                    value => value.HasValue ? new TrainNumber(value.Value) : null)
                .IsRequired(false);
        });
        
        builder.Property(x => x.DestinationName)
            .HasConversion(
                name => name != null ? name.Value : null,
                value => value != null ? StationName.Create(value).Value : null)
            .IsRequired(false);

    }
}