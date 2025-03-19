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

            pb.OwnsOne(p => p.Options, ob =>
            {
                ob.Property(o => o.Reservation);
                ob.Property(o => o.Bikes);
                ob.Property(o => o.Dogs);
                ob.Property(o => o.NeedsAccessibility);
            });

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

        builder.OwnsOne(cr => cr.Options, ob =>
        {
            ob.Property(o => o.ComfortClass);
            ob.Property(o => o.MaxTransfers);
            ob.Property(o => o.MinTransferMinutes);
        });

        builder.Ignore(cr => cr.Route);

        builder.OwnsOne(cr => cr.Route, rb =>
        {
            rb.Property(r => r.DepartureStop)
                .HasConversion(
                    evaNumber => evaNumber.Value,
                    value => EvaNumber.Create(value).Value);

            rb.OwnsOne(r => r.AllowedOnFirstSection);

            rb.OwnsOne(r => r.FirstStopOver, sob =>
            {
                sob.Property(so => so.Stop)
                    .HasConversion(
                        evaNumber => evaNumber.Value,
                        value => EvaNumber.Create(value).Value);

                sob.Property(so => so.StayMinutes);
            });
            
            rb.OwnsOne(r => r.AllowedOnSecondSection);
            
            rb.OwnsOne(r => r.SecondStopOver, sob =>
            {
                sob.Property(so => so.Stop)
                    .HasConversion(
                        evaNumber => evaNumber.Value,
                        value => EvaNumber.Create(value).Value);

                sob.Property(so => so.StayMinutes);
            });
            
            rb.OwnsOne(r => r.AllowedOnThirdSection);
            
            rb.Property(r => r.ArrivalStop)
                .HasConversion(
                    evaNumber => evaNumber.Value,
                    value => EvaNumber.Create(value).Value);
        });
    }
}