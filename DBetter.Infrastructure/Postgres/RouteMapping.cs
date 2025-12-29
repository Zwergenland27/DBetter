using DBetter.Domain.Routes;
using DBetter.Domain.Routes.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class RouteMapping : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
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
                value => new BahnJourneyId(value));

        builder.OwnsMany(x => x.Messages, mb =>
        {
            mb.ToTable("Messages");
            
            mb.WithOwner().HasForeignKey("RouteId");

            mb.Property(x => x.Code);

            mb.Property(x => x.Text);
        });

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