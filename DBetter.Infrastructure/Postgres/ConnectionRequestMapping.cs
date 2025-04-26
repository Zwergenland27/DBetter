using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.ConnectionRequests.Entities;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class ConnectionRequestMapping : IEntityTypeConfiguration<ConnectionRequest>
{
    public void Configure(EntityTypeBuilder<ConnectionRequest> builder)
    {
        builder.ToTable("ConnectionRequests");
        
        builder.HasKey(cr => cr.Id);

        builder.Property(cr => cr.Id)
            .HasConversion(
                id => id.Value,
                value => new ConnectionRequestId(value));

        builder.Property(cr => cr.OwnerId)
            .HasConversion(
                id => id != null ? id.Value : (Guid?) null,
                value => value.HasValue ? new UserId(value.Value) : null)
            .IsRequired(false);

        builder.Property(cr => cr.DepartureTime)
            .IsRequired(false);
        
        builder.Property(cr => cr.ArrivalTime)
            .IsRequired(false);
        
        builder.OwnsMany(cr => cr.Passengers, pb =>
        {
            pb.ToTable("ConnectionRequestPassengers");
            
            pb.WithOwner().HasForeignKey("ConnectionRequestId");

            pb.HasKey(nameof(Passenger.Id), "ConnectionRequestId");
            
            pb.Property(p => p.Id)
                .HasColumnName("PassengerId")
                .HasConversion(
                    id => id.Value,
                    value => new PassengerId(value));
            
            pb.Property(p => p.UserId)
                .HasConversion(
                    id => id != null ? id.Value : (Guid?) null,
                    value => value.HasValue ? new UserId(value.Value) : null)
                .IsRequired(false);
            
            pb.Property(p => p.Name)
                .HasConversion(
                    name => name != null ? name.Value : null,
                    value => value != null ? new PassengerName(value) : null)
                .IsRequired(false);
            
            pb.Property(p => p.Birthday)
                .HasConversion(
                    birthday => birthday != null ? birthday.Utc : (DateTime?) null,
                    value => value.HasValue ? Birthday.Create(value.Value).Value : null)
                .IsRequired(false);
            
            pb.Property(p => p.Age)
                .IsRequired(false);

            pb.Property(p => p.Bikes);

            pb.Property(p => p.Dogs);

            pb.OwnsMany(p => p.Discounts, db =>
            {
                db.ToTable("ConnectionRequestPassengerDiscounts");
                
                db.WithOwner().HasForeignKey("PassengerId", "ConnectionRequestId");
                
                db.Property<Guid>("Id");
                db.HasKey("Id", "PassengerId", "ConnectionRequestId");
                
                db.Property(discount => discount.Type);
                db.Property(discount => discount.ComfortClass);
                db.Property(discount => discount.ValidUntil);
                
            }).UsePropertyAccessMode(PropertyAccessMode.Field);
        })
        .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Property(o => o.ComfortClass);

        builder.Ignore(cr => cr.Route);

        builder.OwnsOne(cr => cr.Route, rb =>
        {
            rb.Property(r => r.OriginStationId)
                .HasConversion(
                    id => id.Value,
                    value => new StationId(value));

            rb.OwnsOne(so => so.MeansOfTransportFirstSection, motb =>
            {
                motb.Ignore(mot => mot.AnySelected);
            });
            
            rb.OwnsOne(r => r.FirstStopover, sob =>
             {
                 sob.Property(so => so.StationId)
                     .HasConversion(
                         id => id.Value,
                         value => new StationId(value));
            
                 sob.Property(so => so.LengthOfStay);
            
                 sob.OwnsOne(so => so.MeansOfTransportNextSection, motb =>
                 {
                     motb.Ignore(mot => mot.AnySelected);
                 });
             });
            
            rb.OwnsOne(r => r.SecondStopover, sob =>
            {
                sob.Property(so => so.StationId)
                    .HasConversion(
                        id => id.Value,
                        value => new StationId(value));
            
                sob.Property(so => so.LengthOfStay);
                
                sob.OwnsOne(so => so.MeansOfTransportNextSection, motb =>
                {
                    motb.Ignore(mot => mot.AnySelected);
                });
            });
            
            rb.Property(r => r.DestinationStationId)
                .HasConversion(
                    id => id.Value,
                    value => new StationId(value));

            rb.Property(r => r.MaxTransfers);
            rb.Property(r => r.MinTransferTime);
        });
    }
}