using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.ConnectionRequests.Commands.Put;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Parameters;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests.Entities;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections;
using DBetter.Domain.Errors;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.Users.ValueObjects;
using ICommand = DBetter.Application.Abstractions.Messaging.ICommand;

namespace DBetter.Application.Connections.Queries.GetSuggestions;

public class GetConnectionSuggestionsQueryBuilder : IRequestBuilder<ConnectionRequestParameters, GetConnectionSuggestionsQuery>{
    public ValidatedRequiredProperty<GetConnectionSuggestionsQuery> Configure(RequiredPropertyBuilder<ConnectionRequestParameters, GetConnectionSuggestionsQuery> builder)
    {
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
            .Required()
            .MapEachComplex(r => r.Passengers, MapPassenger);

        var options = builder.ClassProperty(r => r.Options)
            .Required()
            .MapComplex(p => p.Options, MapOptions);
        
        var route = builder.ClassProperty(r => r.Route)
            .Required()
            .MapComplex(p => p.Route, MapRoute);

        var page = builder.ClassProperty(r => r.Page)
            .Optional()
            .Map(p => p.Page);
        
        return builder.Build(() => new GetConnectionSuggestionsQuery(
            ownerId,
            departureTime,
            arrivalTime,
            passengers.ToList(),
            options,
            route,
            page));
    }
    
    private ValidatedRequiredProperty<Passenger> MapPassenger(RequiredPropertyBuilder<PassengerParameters, Passenger> builder)
    {
        var id = builder.ClassProperty(r => r.Id)
            .Required()
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
            .Required()
            .MapComplex(p => p.Options, pBuilder =>
            {
                var reservation = pBuilder.StructProperty(r => r.Reservation)
                    .Required()
                    .Map(p => p.Reservation);
                
                var bikes = pBuilder.StructProperty(r => r.Bikes)
                    .Required()
                    .Map(p => p.Bikes);
                
                var dogs = pBuilder.StructProperty(r => r.Dogs)
                    .Required()
                    .Map(p => p.Dogs);
                
                var needsAccessibility = pBuilder.StructProperty(r => r.NeedsAccessibility)
                    .Required()
                    .Map(p => p.NeedsAccessibility);
                
                return pBuilder.Build(() => new PassengerOptions(reservation, bikes, dogs, needsAccessibility));
            });

        var discounts = builder.ListProperty(r => r.Discounts)
            .Required()
            .MapEachComplex(r => r.Discounts, dBuilder =>
            {
                var type = dBuilder.EnumProperty(r => r.Type)
                    .Required()
                    .Map(r => r.Type, DomainErrors.Shared.DiscountType.Invalid);
                
                var comfortClass = dBuilder.EnumProperty(r => r.ComfortClass)
                    .Required()
                    .Map(r => r.Class, DomainErrors.Shared.Class.Invalid);
                
                var validUntil = dBuilder.StructProperty(r => r.ValidUntil)
                    .Optional()
                    .Map(r => r.ValidUntil);
                
                return dBuilder.Build(() => new PassengerDiscount(type, comfortClass, validUntil));
            });

        return builder.Build(() => Passenger.Create(id, userId, name, birthday, age, options, discounts.ToList()));
    }

    private ValidatedRequiredProperty<ConnectionOptions> MapOptions(
        RequiredPropertyBuilder<ConnectionOptionsParameters, ConnectionOptions> builder)
    {
        var comfortClass = builder.EnumProperty(r => r.ComfortClass)
            .Required()
            .Map(r => r.Class, DomainErrors.Shared.Class.Invalid);
        
        var maxTransfers = builder.StructProperty(r => r.MaxTransfers)
            .Required()
            .Map(r => r.MaxTransfers);
        
        var minTransferMinutes = builder.StructProperty(r => r.MinTransferMinutes)
            .Required()
            .Map(r => r.MinTransferMinutes);
        
        return builder.Build(() => new ConnectionOptions(comfortClass, maxTransfers, minTransferMinutes));
    }

    private ValidatedRequiredProperty<Route> MapRoute(
        RequiredPropertyBuilder<ConnectionRouteParameters, Route> builder)
    {
        var stops = builder.ListProperty<EvaNumber>("Stops")
            .Required()
            .MapEach(p => p.Stops, EvaNumber.Create);

        var allowedVehicles = builder.ListProperty<AllowedVehicles>("AllowedVehicles")
            .Required()
            .MapEachComplex(p => p.AllowedVehicles, vBuilder =>
            {
                var highSpeed = vBuilder.StructProperty(r => r.HighSpeed)
                    .Required()
                    .Map(p => p.HighSpeed);

                var intercity = vBuilder.StructProperty(r => r.Intercity)
                    .Required()
                    .Map(p => p.Intercity);

                var regional = vBuilder.StructProperty(r => r.Regional)
                    .Required()
                    .Map(p => p.Regional);

                var publicTransport = vBuilder.StructProperty(p => p.PublicTransport)
                    .Required()
                    .Map(p => p.PublicTransport);

                return vBuilder.Build(() => new AllowedVehicles(highSpeed, intercity, regional, publicTransport));
            });
        
        return builder.Build(() => Route.Create(stops.ToList(), allowedVehicles.ToList()));
    }
}

public record GetConnectionSuggestionsQuery(
    UserId? OwnerId,
    DateTime? DepartureTime,
    DateTime? ArrivalTime,
    List<Passenger> Passengers,
    ConnectionOptions Options,
    Route Route,
    string? Page) : ICommand<ConnectionSuggestionsDto>;