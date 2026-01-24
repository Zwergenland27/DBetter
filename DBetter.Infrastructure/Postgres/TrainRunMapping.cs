using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.Entities;
using DBetter.Domain.TrainRuns.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class TrainRunMapping : IEntityTypeConfiguration<TrainRun>
{
    public void Configure(EntityTypeBuilder<TrainRun> builder)
    {
        builder.ToTable("TrainRuns");
        
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.JourneyId)
            .IsUnique();
        
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new TrainRunId(value));

        builder.Property(x => x.JourneyId)
            .HasConversion(
                id => id.Value,
                value => new BahnJourneyId(value));

        builder.OwnsMany(x => x.PassengerInformation, mb =>
        {
            mb.ToTable("TrainRunPassengerInformation");
            
            mb.WithOwner().HasForeignKey("TrainRunId");

            mb.HasKey("TrainRunId", nameof(TrainRunPassengerInformation.Id));

            mb.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,
                    value => new TrainRunPassengerInformationId(value));
            
            mb.Property(x => x.InformationId)
                .HasConversion(
                    id => id.Value,
                    value => new PassengerInformationId(value));
            
            mb.Property(x => x.FromStopIndex)
                .HasConversion(
                    stopIndex => stopIndex.Value,
                    value => new StopIndex(value));
            
            mb.Property(x => x.ToStopIndex)
                .HasConversion(
                    stopIndex => stopIndex.Value,
                    value => new StopIndex(value));

        }).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsOne(x => x.Catering, cib =>
        {
            cib.Property(x => x.Type);
            
            cib.Property(x => x.FromStopIndex)
                .HasConversion(
                    stopIndex => stopIndex.Value,
                    value => new StopIndex(value))
                .IsRequired(false);
            
            cib.Property(x => x.ToStopIndex)
                .HasConversion(
                    stopIndex => stopIndex.Value,
                    value => new StopIndex(value))
                .IsRequired(false);
        });
        
        builder.OwnsOne(x => x.BikeCarriage, cib =>
        {
            cib.Property(x => x.Status);
            
            cib.Property(x => x.FromStopIndex)
                .HasConversion(
                    stopIndex => stopIndex.Value,
                    value => new StopIndex(value))
                .IsRequired(false);
            
            cib.Property(x => x.ToStopIndex)
                .HasConversion(
                    stopIndex => stopIndex.Value,
                    value => new StopIndex(value))
                .IsRequired(false);
        });
        
        
        builder.OwnsOne(x => x.ServiceInformation, tib =>
        {
            tib.Property(x => x.ProductClass);

            tib.Property(x => x.TransportCategory);

            tib.Property(x => x.LineNumber)
                .HasConversion(
                    line => line != null ? line.Value : null,
                    value => value != null ? new LineNumber(value) : null)
                .IsRequired(false);
            
            tib.Property(x => x.ServiceNumber)
                .HasConversion(
                    number => number != null ? number.Value : (int?) null,
                    value => value.HasValue ? new ServiceNumber(value.Value) : null)
                .IsRequired(false);
        });
    }
}