using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Parameters;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests.Entities;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Errors;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.Users.ValueObjects;

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

        var options = builder.EnumProperty(r => r.ComfortClass)
            .Required()
            .Map(p => p.ComfortClass, DomainErrors.Shared.ComfortClass.Invalid);
        
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
        
        var bikes = builder.StructProperty(r => r.Bikes)
            .Required()
            .Map(p => p.Bikes);
                
        var dogs = builder.StructProperty(r => r.Dogs)
            .Required()
            .Map(p => p.Dogs);

        var discounts = builder.ListProperty(r => r.Discounts)
            .Required()
            .MapEachComplex(r => r.Discounts, dBuilder =>
            {
                var type = dBuilder.EnumProperty(r => r.Type)
                    .Required()
                    .Map(r => r.Type, DomainErrors.Shared.DiscountType.Invalid);
                
                var comfortClass = dBuilder.EnumProperty(r => r.ComfortClass)
                    .Required()
                    .Map(r => r.ComfortClass, DomainErrors.Shared.ComfortClass.Invalid);
                
                var validUntil = dBuilder.StructProperty(r => r.ValidUntil)
                    .Optional()
                    .Map(r => r.ValidUntil);
                
                return dBuilder.Build(() => new PassengerDiscount(type, comfortClass, validUntil));
            });

        return builder.Build(() => Passenger.Create(id, userId, name, birthday, age, bikes, dogs, discounts.ToList()));
    }

    private ValidatedRequiredProperty<Route> MapRoute(
        RequiredPropertyBuilder<ConnectionRouteParameters, Route> builder)
    {
        var originStationId = builder.ClassProperty(r => r.OriginStationId)
            .Required()
            .Map(p => p.OriginStationId, StationId.Create);
       
        var meansOfTransportFirstSection = builder.ClassProperty(r => r.MeansOfTransportFirstSection)
            .Required()
            .MapComplex(p => p.MeansOfTransportFirstSection, MapMeansOfTransport);

        var firstStopover = builder.ClassProperty(r => r.FirstStopover)
            .Optional()
            .MapComplex(p => p.FirstStopover, MapStopover);
        
        var secondStopover = builder.ClassProperty(r => r.SecondStopover)
            .Optional()
            .MapComplex(p => p.SecondStopover, MapStopover);
        
        var destinationStationId = builder.ClassProperty(r => r.DestinationStationId)
            .Required()
            .Map(p => p.DestinationStationId, StationId.Create);

        var maxTransfers = builder.StructProperty(r => r.MaxTransfers)
            .Required()
            .Map(p => p.MaxTransfers);
        
        var minTransferTime = builder.StructProperty(r => r.MinTransferTime)
            .Required()
            .Map(p => p.MinTransferTime);
        
        return builder.Build(() => Route.Create(
            originStationId,
            meansOfTransportFirstSection,
            firstStopover,
            secondStopover,
            destinationStationId,
            maxTransfers,
            minTransferTime));
    }

    private ValidatedRequiredProperty<MeansOfTransport> MapMeansOfTransport(
        RequiredPropertyBuilder<MeansOfTransportParameters, MeansOfTransport> builder)
    {
        var highSpeedTrains = builder.StructProperty(r => r.HighSpeedTrains)
            .Required()
            .Map(p => p.HighSpeedTrains);
        
        var fastTrains = builder.StructProperty(r => r.FastTrains)
            .Required()
            .Map(p => p.FastTrains);
        
        var regionalTrains = builder.StructProperty(r => r.RegionalTrains)
            .Required()
            .Map(p => p.RegionalTrains);
        
        var suburbanTrains = builder.StructProperty(r => r.SuburbanTrains)
            .Required()
            .Map(p => p.SuburbanTrains);
        
        var undergroundTrains = builder.StructProperty(r => r.UndergroundTrains)
            .Required()
            .Map(p => p.UndergroundTrains);
        
        var trams = builder.StructProperty(r => r.Trams)
            .Required()
            .Map(p => p.Trams);
        
        var busses = builder.StructProperty(r => r.Busses)
            .Required()
            .Map(p => p.Busses);
        
        var boats = builder.StructProperty(r => r.Boats)
            .Required()
            .Map(p => p.Boats);
        
        return builder.Build(() => new MeansOfTransport(
            highSpeedTrains,
            fastTrains,
            regionalTrains,
            suburbanTrains,
            undergroundTrains,
            trams,
            busses,
            boats));
    }

    private ValidatedOptionalClassProperty<Stopover> MapStopover(
        OptionalClassPropertyBuilder<ConnectionRouteStopoverParameters, Stopover> builder)
    {
        var stationId = builder.ClassProperty(r => r.StationId)
            .Required()
            .Map(p => p.StationId, StationId.Create);
        
        var lengthOfStay = builder.StructProperty(r => r.LengthOfStay)
            .Required()
            .Map(p => p.LengthOfStay);
        
        var meansOfTransportNextSection = builder.ClassProperty(r => r.MeansOfTransportNextSection)
            .Required()
            .MapComplex(p => p.MeansOfTransportNextSection, MapMeansOfTransport);
        
        return builder.Build(() => new Stopover(stationId, lengthOfStay, meansOfTransportNextSection));
    }
}

public record GetConnectionSuggestionsQuery(
    UserId? OwnerId,
    DateTime? DepartureTime,
    DateTime? ArrivalTime,
    List<Passenger> Passengers,
    ComfortClass ComfortClass,
    Route Route,
    string? Page) : ICommand<ConnectionSuggestionsDto>;