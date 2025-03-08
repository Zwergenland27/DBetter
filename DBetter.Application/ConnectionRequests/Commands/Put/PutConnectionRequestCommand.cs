using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Errors;
using DBetter.Contracts.ConnectionRequests.Commands.Put;
using DBetter.Domain.ConnectionRequests.Entities;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Errors;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.Users.ValueObjects;
using ICommand = DBetter.Application.Abstractions.Messaging.ICommand;

namespace DBetter.Application.ConnectionRequests.Commands.Put;

public class PutConnectionRequestRequestBuilder : IRequestBuilder<ConnectionRequestParameters, PutConnectionRequestCommand>{
    public ValidatedRequiredProperty<PutConnectionRequestCommand> Configure(RequiredPropertyBuilder<ConnectionRequestParameters, PutConnectionRequestCommand> builder)
    {
        var id = builder.ClassProperty(r => r.Id)
            .Required(ApplicationErrors.ConnectionRequest.Put.Id.Missing)
            .Map(p => p.Id, ConnectionRequestId.Create);
        
        var ownerId = builder.ClassProperty(r => r.OwnerId)
            .Optional()
            .Map(p => p.OwnerId, UserId.Create);
        
        var departureTime = builder.StructProperty(r => r.DepartureTime)
            .Optional()
            .Map(p => p.DepartureTime);
        
        var arrivalTime = builder.StructProperty(r => r.ArrivalTime)
            .Optional()
            .Map(p => p.ArrivalTime);

        var passengers = builder.ListProperty(r => r.Passengers)
            .Required(ApplicationErrors.ConnectionRequest.Put.Passengers.Missing)
            .MapEachComplex(r => r.Passengers, MapPassenger);

        var options = builder.ClassProperty(r => r.Options)
            .Required(ApplicationErrors.ConnectionRequest.Put.Options.Missing)
            .MapComplex(p => p.Options, MapOptions);
        
        var route = builder.ClassProperty(r => r.Route)
            .Required(ApplicationErrors.ConnectionRequest.Put.Route.Missing)
            .MapComplex(p => p.Route, MapRoute);
        
        return builder.Build(() => new PutConnectionRequestCommand(
            id,
            ownerId,
            departureTime,
            arrivalTime,
            passengers.ToList(),
            options,
            route));
    }
    
    private ValidatedRequiredProperty<Passenger> MapPassenger(RequiredPropertyBuilder<PassengerParameters, Passenger> builder)
    {
        var id = builder.ClassProperty(r => r.Id)
            .Required(ApplicationErrors.ConnectionRequest.Put.Passengers.Id.Missing)
            .Map(p => p.Id, PassengerId.Create);
        
        var userId = builder.ClassProperty(r => r.UserId)
            .Optional()
            .Map(p => p.UserId, UserId.Create);

        var name = builder.ClassProperty(r => r.Name)
            .Optional()
            .Map(p => p.Name, value => new PassengerName(value));

        var birthday = builder.ClassProperty(r => r.Birthday)
            .Optional()
            .Map(p => p.Birthday, Birthday.Create);

        var age = builder.StructProperty(r => r.Age)
            .Optional()
            .Map(p => p.Age);
        
        var options = builder.ClassProperty(r => r.Options)
            .Required(ApplicationErrors.ConnectionRequest.Put.Passengers.Options.Missing)
            .MapComplex(p => p.Options, pBuilder =>
            {
                var reservation = pBuilder.StructProperty(r => r.Reservation)
                    .Required(ApplicationErrors.ConnectionRequest.Put.Passengers.Options.Reservation.Missing)
                    .Map(p => p.Reservation);
                
                var bikes = pBuilder.StructProperty(r => r.Bikes)
                    .Required(ApplicationErrors.ConnectionRequest.Put.Passengers.Options.Bikes.Missing)
                    .Map(p => p.Bikes);
                
                var dogs = pBuilder.StructProperty(r => r.Dogs)
                    .Required(ApplicationErrors.ConnectionRequest.Put.Passengers.Options.Dogs.Missing)
                    .Map(p => p.Dogs);
                
                var needsAccessibility = pBuilder.StructProperty(r => r.NeedsAccessibility)
                    .Required(ApplicationErrors.ConnectionRequest.Put.Passengers.Options.NeedsAccessibility.Missing)
                    .Map(p => p.NeedsAccessibility);
                
                return pBuilder.Build(() => new PassengerOptions(reservation, bikes, dogs, needsAccessibility));
            });

        var discounts = builder.ListProperty(r => r.Discounts)
            .Required(ApplicationErrors.ConnectionRequest.Put.Passengers.Discounts.Missing)
            .MapEachComplex(r => r.Discounts, dBuilder =>
            {
                var type = dBuilder.EnumProperty(r => r.Type)
                    .Required(ApplicationErrors.ConnectionRequest.Put.Passengers.Discounts.Type.Missing)
                    .Map(r => r.Type, DomainErrors.Shared.DiscountType.Invalid);
                
                var @class = dBuilder.EnumProperty(r => r.Class)
                    .Required(ApplicationErrors.ConnectionRequest.Put.Passengers.Discounts.Class.Missing)
                    .Map(r => r.Class, DomainErrors.Shared.Class.Invalid);
                
                var validUntil = dBuilder.StructProperty(r => r.ValidUntil)
                    .Optional()
                    .Map(r => r.ValidUntil);
                
                return dBuilder.Build(() => new PassengerDiscount(type, @class, validUntil));
            });

        return builder.Build(() => Passenger.Create(id, userId, name, birthday, age, options, discounts.ToList()));
    }

    private ValidatedRequiredProperty<ConnectionOptions> MapOptions(
        RequiredPropertyBuilder<ConnectionOptionsParameters, ConnectionOptions> builder)
    {
        var @class = builder.EnumProperty(r => r.Class)
            .Required(ApplicationErrors.ConnectionRequest.Put.Options.Class.Missing)
            .Map(r => r.Class, DomainErrors.Shared.Class.Invalid);
        
        var maxTransfers = builder.StructProperty(r => r.MaxTransfers)
            .Required(ApplicationErrors.ConnectionRequest.Put.Options.MaxTransfers.Missing)
            .Map(r => r.MaxTransfers);
        
        var minTransferMinutes = builder.StructProperty(r => r.MinTransferMinutes)
            .Required(ApplicationErrors.ConnectionRequest.Put.Options.MinTransferMinutes.Missing)
            .Map(r => r.MinTransferMinutes);
        
        return builder.Build(() => new ConnectionOptions(@class, maxTransfers, minTransferMinutes));
    }

    private ValidatedRequiredProperty<Route> MapRoute(
        RequiredPropertyBuilder<ConnectionRouteParameters, Route> builder)
    {
        var stops = builder.ListProperty(r => r.Stops)
            .Required(ApplicationErrors.ConnectionRequest.Put.Route.Stops.Missing)
            .MapEach(p => p.Stops, EvaNumber.Create);

        var allowedVehicles = builder.ListProperty(r => r.AllowedVehicles)
            .Required(ApplicationErrors.ConnectionRequest.Put.Route.AllowedVehicles.Missing)
            .MapEachComplex(p => p.AllowedVehicles, vBuilder =>
            {
                var highSpeed = vBuilder.StructProperty(r => r.HighSpeed)
                    .Required(ApplicationErrors.ConnectionRequest.Put.Route.AllowedVehicles.HighSpeed.Missing)
                    .Map(p => p.HighSpeed);

                var intercity = vBuilder.StructProperty(r => r.Intercity)
                    .Required(ApplicationErrors.ConnectionRequest.Put.Route.AllowedVehicles.Intercity.Missing)
                    .Map(p => p.Intercity);

                var regional = vBuilder.StructProperty(r => r.Regional)
                    .Required(ApplicationErrors.ConnectionRequest.Put.Route.AllowedVehicles.Regional.Missing)
                    .Map(p => p.Regional);

                var publicTransport = vBuilder.StructProperty(p => p.PublicTransport)
                    .Required(ApplicationErrors.ConnectionRequest.Put.Route.AllowedVehicles.PublicTransport.Missing)
                    .Map(p => p.PublicTransport);

                return vBuilder.Build(() => new AllowedVehicles(highSpeed, intercity, regional, publicTransport));
            });
        
        return builder.Build(() => Route.Create(stops.ToList(), allowedVehicles.ToList()));
    }
}

public record PutConnectionRequestCommand(
    ConnectionRequestId Id,
    UserId? OwnerId,
    DateTime? DepartureTime,
    DateTime? ArrivalTime,
    List<Passenger> Passengers,
    ConnectionOptions Options,
    Route Route) : ICommand;