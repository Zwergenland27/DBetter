using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCirculations.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class TrainCirculationMapping : IEntityTypeConfiguration<TrainCirculation>
{
    public void Configure(EntityTypeBuilder<TrainCirculation> builder)
    {
        builder.ToTable("TrainCirculations");
        
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.NormalizedJourneyId)
            .IsUnique();

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new TrainCirculationId(value));

        builder.Property(x => x.NormalizedJourneyId)
            .HasConversion(
                id => id.Value,
                value => NormalizedBahnJourneyId.CreateNormalized(value));
        
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