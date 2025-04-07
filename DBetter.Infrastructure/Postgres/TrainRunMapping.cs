using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.Entities;
using DBetter.Infrastructure.BahnDe.Routes.DTOs;
using DBetter.Infrastructure.BahnDe.Routes.Entities;
using DBetter.Infrastructure.BahnDe.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class RouteMapping : IEntityTypeConfiguration<RouteEntity>
{
    public void Configure(EntityTypeBuilder<RouteEntity> builder)
    {
        builder.ToTable("Routes");
        
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.JourneyId)
            .IsUnique();
        
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new RouteId(value));

        builder.Property(x => x.JourneyId)
            .HasConversion(
                id => id.Value,
                value => new JourneyId(value));
        
        builder.OwnsOne(x => x.Information, tib =>
        {
            tib.Property(x => x.Product);

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

        builder.Property<bool>("_hasBeenScraped");
        
        builder.Property(x => x.ScrapingRequired);
    }
}