using DBetter.Domain.Journey;
using DBetter.Domain.TrainRun.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class TrainRunMapping : IEntityTypeConfiguration<TrainRun>
{
    public void Configure(EntityTypeBuilder<TrainRun> builder)
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

        builder.Ignore(x => x.Messages);
        
        builder.Ignore(x => x.Catering);

        builder.Ignore(x => x.BikeCarriage);
        
        builder.Ignore(x => x.Train);

        builder.Ignore(x => x.Stops);

    }
}