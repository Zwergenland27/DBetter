using DBetter.Domain.Journey;
using DBetter.Domain.Journey.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class JourneyMapping : IEntityTypeConfiguration<EmptyJourney>
{
    public void Configure(EntityTypeBuilder<EmptyJourney> builder)
    {
        builder.ToTable("EmptyJourneys");
        
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.BahnId)
            .IsUnique();
        
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new JourneyId(value));

        builder.Property(x => x.BahnId)
            .HasConversion(
                id => id.Value,
                value => new BahnJourneyId(value));

    }
}