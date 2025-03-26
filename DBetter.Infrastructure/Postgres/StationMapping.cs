using DBetter.Domain.Errors;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class StationMapping : IEntityTypeConfiguration<Station>
{
    public void Configure(EntityTypeBuilder<Station> builder)
    {
        builder.ToTable("Stations");
        
        builder.HasKey(station => station.Id);
        
        builder.HasIndex(station => station.EvaNumber)
            .IsUnique();

        builder.Property(station => station.Id)
            .HasConversion(
                id => id.Value,
                value => new StationId(value));
        
        builder.Property(station => station.EvaNumber)
            .HasConversion(
                evaNumber => evaNumber.Value,
                value => EvaNumber.Create(value).Value);
        
        builder.Property(station => station.InfoId)
            .HasConversion(
                infoId => infoId != null ? infoId.Value : null,
                value => value != null ? StationInfoId.Create(value).Value : null)
            .IsRequired(false);
        
        builder.Property(station => station.Name)
            .HasConversion(
                name => name.Value,
                value => StationName.Create(value).Value);

        builder.OwnsOne(station => station.Position);
    }
}