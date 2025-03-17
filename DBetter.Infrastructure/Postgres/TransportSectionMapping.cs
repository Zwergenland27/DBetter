using DBetter.Domain.Connections.Entities;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class TransportSectionMapping : IEntityTypeConfiguration<TransportSection>
{
    public void Configure(EntityTypeBuilder<TransportSection> builder)
    {
        builder.ToTable("TransportSections");
        
        builder.Property(x => x.JourneyId)
            .HasConversion(
                id => id.Value,
                value => new JourneyId(value));

        builder.OwnsOne(x => x.Demand);

        builder.OwnsOne(x => x.Information, ib =>
        {
            ib.Property(x => x.MeansOfTransport);

            ib.OwnsOne(x => x.TrainName);

            ib.OwnsOne(x => x.Catering, cb =>
            {
                cb.Property(x => x.Type);
                
                cb.Property(x => x.FromStopIndex)
                    .HasConversion(
                        index => index != null ? index.Value : (int?) null,
                        value => value.HasValue ? new StopIndex(value.Value) : null)
                    .IsRequired(false);
                
                cb.Property(x => x.ToStopIndex)
                    .HasConversion(
                        index => index != null ? index.Value : (int?) null,
                        value => value.HasValue ? new StopIndex(value.Value) : null)
                    .IsRequired(false);
            });

            ib.OwnsOne(x => x.BikeTransport, bb =>
            {
                bb.Property(x => x.Status);
                
                bb.Property(x => x.FromStopIndex)
                    .HasConversion(
                        index => index != null ? index.Value : (int?) null,
                        value => value.HasValue ? new StopIndex(value.Value) : null)
                    .IsRequired(false);
                
                bb.Property(x => x.ToStopIndex)
                    .HasConversion(
                        index => index != null ? index.Value : (int?) null,
                        value => value.HasValue ? new StopIndex(value.Value) : null)
                    .IsRequired(false);
            });
        });

        builder.OwnsMany(x => x.Messages, mb =>
            {
                mb.ToTable("SectionMessages");
                
                mb.WithOwner().HasForeignKey("SectionId");
                
                mb.Property<Guid>("Id");
                
                mb.HasKey("Id", "SectionId");

                mb.Property(x => x.Code);

                mb.Property(x => x.DefaultText);

                mb.Property(x => x.FromStopIndex)
                    .HasConversion(
                        index => index != null ? index.Value : (int?)null,
                        value => value.HasValue ? new StopIndex(value.Value) : null);
                
                mb.Property(x => x.ToStopIndex)
                    .HasConversion(
                        index => index != null ? index.Value : (int?)null,
                        value => value.HasValue ? new StopIndex(value.Value) : null);
            })
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.OwnsMany(x => x.Stops, sb =>
        {
            sb.ToTable("SectionStops");
            
            sb.WithOwner().HasForeignKey("SectionId");
            
            sb.Property<Guid>("Id");
                
            sb.HasKey("Id", "SectionId");

            sb.Property(x => x.StationId)
                .HasConversion(
                    id => id.Value,
                    value => new StationId(value));
            
            sb.Property(x => x.StopIndex)
                .HasConversion(
                    index => index.Value,
                    value => new StopIndex(value));

            sb.OwnsOne(x => x.Platform);

            sb.OwnsOne(x => x.Demand);

            sb.OwnsOne(x => x.DepartureTime);
            
            sb.OwnsOne(x => x.ArrivalTime);

            sb.Property(x => x.IsAdditional);
            
            sb.Property(x => x.IsCancelled);

            sb.Property(x => x.IsExitOnly);
            
            sb.Property(x => x.IsEntryOnly);
            
            sb.OwnsMany(x => x.Messages, mb =>
                {
                    mb.ToTable("SectionStopMessages");
                
                    mb.WithOwner().HasForeignKey("StopId", "SectionId");
                
                    mb.Property<Guid>("Id");
                
                    mb.HasKey("Id", "SectionId", "StopId");

                    mb.Property(x => x.Code);

                    mb.Property(x => x.DefaultText);
                })
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        })
        .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}